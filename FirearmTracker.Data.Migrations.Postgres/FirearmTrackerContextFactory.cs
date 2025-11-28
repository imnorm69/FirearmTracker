using FirearmTracker.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FirearmTracker.Data.Migrations.Postgres
{
    public class FirearmTrackerContextFactory : IDesignTimeDbContextFactory<FirearmTrackerContext>
    {
        public FirearmTrackerContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FirearmTrackerContext>();

            optionsBuilder.UseNpgsql("Host=localhost;Database=dummy;Username=dummy;Password=dummy",
                x => x.MigrationsAssembly("FirearmTracker.Data.Migrations.Postgres"));

            return new FirearmTrackerContext(optionsBuilder.Options);
        }
    }
}