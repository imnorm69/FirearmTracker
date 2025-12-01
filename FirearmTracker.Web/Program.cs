using FirearmTracker.Core.Enums;
using FirearmTracker.Core.Models;
using FirearmTracker.Core.Interfaces;
using FirearmTracker.Data.Context;
using FirearmTracker.Data.Repositories;
using FirearmTracker.Web.Components;
using FirearmTracker.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Razor / Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Enable detailed logging for antiforgery
builder.Logging.AddFilter("Microsoft.AspNetCore.Antiforgery", LogLevel.Debug);
builder.Logging.AddFilter("Microsoft.AspNetCore.Components.Forms", LogLevel.Debug);

// Configuration Services
builder.Services.AddSingleton<IDatabaseConfigurationService, DatabaseConfigurationService>();
builder.Services.AddSingleton<IHealthCheckService, HealthCheckService>();

// Database - Configure based on dbconfig.json
builder.Services.AddDbContext<FirearmTrackerContext>((serviceProvider, options) =>
{
    var environment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
    var configFilePath = Path.Combine(environment.ContentRootPath, "dbconfig.json");

    DatabaseConfiguration? dbConfig = null;
    if (File.Exists(configFilePath))
    {
        var json = File.ReadAllText(configFilePath);
        dbConfig = System.Text.Json.JsonSerializer.Deserialize<DatabaseConfiguration>(json);
    }

    if (dbConfig == null)
    {
        // No configuration exists yet - use SQLite as default for initial setup
        options.UseSqlite("Data Source=firearmtracker.db",
            x => x.MigrationsAssembly("FirearmTracker.Data.Migrations.Sqlite"));
    }
    else
    {
        var connectionString = dbConfig.GetConnectionString();

        if (dbConfig.DatabaseType == DatabaseType.Sqlite)
        {
            options.UseSqlite(connectionString,
                x => x.MigrationsAssembly("FirearmTracker.Data.Migrations.Sqlite"));
        }
        else if (dbConfig.DatabaseType == DatabaseType.Postgres)
        {
            options.UseNpgsql(connectionString,
                x => x.MigrationsAssembly("FirearmTracker.Data.Migrations.Postgres"));
        }
    }
});

// Repositories
builder.Services.AddScoped<IFirearmRepository, FirearmRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IAccessoryRepository, AccessoryRepository>();
builder.Services.AddScoped<IAmmunitionRepository, AmmunitionRepository>();
builder.Services.AddScoped<IAmmunitionTransactionRepository, AmmunitionTransactionRepository>();
builder.Services.AddScoped<IFirearmAmmunitionLinkRepository, FirearmAmmunitionLinkRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Services
builder.Services.AddScoped<FileUploadService>();
builder.Services.AddScoped<FirearmOwnershipService>();
builder.Services.AddScoped<CaliberService>();
builder.Services.AddScoped<IconService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAvatarService, AvatarService>();
builder.Services.AddScoped<IBackupRestoreService, BackupRestoreService>();

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.SameSite = SameSiteMode.Lax;  // CRITICAL: Allow cross-device access
        options.Cookie.Name = "FirearmTracker.Auth";  // Explicit cookie name
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// Run initial health check
using (var scope = app.Services.CreateScope())
{
    var healthCheck = scope.ServiceProvider.GetRequiredService<IHealthCheckService>();
    await healthCheck.RunChecksAsync(); // Wait for it to complete
}

//
// 1. APPLY MIGRATIONS BEFORE ANY OTHER LOGIC
//
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<FirearmTrackerContext>();
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred applying database migrations.");

        if (app.Environment.IsDevelopment())
            throw;
    }
}

// Error handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

// Only redirect to HTTPS in Development.
// Your LXC is Production and uses HTTP on port 80.
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

//
// 2. FIRST-TIME SETUP CHECK — SAFE, NO REDIRECT LOOP
//
app.Use(async (context, next) =>
{
    // Skip the check for static files, _framework, and antiforgery endpoints
    if (context.Request.Path.StartsWithSegments("/_framework") ||
        context.Request.Path.StartsWithSegments("/_blazor") ||
        context.Request.Path.StartsWithSegments("/css") ||
        context.Request.Path.StartsWithSegments("/bootstrap") ||
        context.Request.Path.Value?.Contains("antiforgery", StringComparison.OrdinalIgnoreCase) == true)
    {
        await next();
        return;
    }

    // Skip checks if already on setup or database setup pages
    if (context.Request.Path.StartsWithSegments("/setup") ||
        context.Request.Path.StartsWithSegments("/admin/database-setup"))
    {
        await next();
        return;
    }

    // Check for database configuration first
    var dbConfigService = context.RequestServices.GetRequiredService<IDatabaseConfigurationService>();
    var hasDbConfig = await dbConfigService.ConfigurationExistsAsync();

    if (!hasDbConfig)
    {
        context.Response.Redirect("/admin/database-setup");
        return;
    }

    // Then check for first-time user setup
    var authService = context.RequestServices.GetRequiredService<IAuthenticationService>();
    var isFirstTime = await authService.IsFirstTimeSetupAsync();

    if (isFirstTime)
    {
        context.Response.Redirect("/setup");
        return;
    }

    await next();
});

app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Avatar endpoint
app.MapGet("/avatar/{userId:int}", async (int userId, IAvatarService avatarService, HttpContext context) =>
{
    var (imageData, contentType) = await avatarService.GetUserAvatarAsync(userId);

    if (imageData != null && contentType != null)
    {
        context.Response.ContentType = contentType;
        await context.Response.Body.WriteAsync(imageData);
        return Results.Empty;
    }

    return Results.NotFound();
});

// Current user avatar endpoint
app.MapGet("/avatar/current", async (HttpContext context, IAvatarService avatarService, IUserRepository userRepository) =>
{
    var username = context.User.Identity?.Name;
    if (string.IsNullOrEmpty(username))
    {
        return Results.NotFound();
    }

    var user = await userRepository.GetByUsernameAsync(username);
    if (user == null)
    {
        return Results.NotFound();
    }

    var (imageData, contentType) = await avatarService.GetUserAvatarAsync(user.Id);

    if (imageData != null && contentType != null)
    {
        context.Response.ContentType = contentType;
        await context.Response.Body.WriteAsync(imageData);
        return Results.Empty;
    }

    return Results.NotFound();
});

app.Run();