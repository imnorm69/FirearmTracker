# FirearmTracker

![FirearmTracker Logo](assets/firearmtracker-logo-1280x640.png)

A comprehensive personal firearms collection management application built with ASP.NET Core and Blazor Server.

## Overview

FirearmTracker is a self-hosted web application designed to help firearms enthusiasts maintain detailed records of their collection, including:

- **Firearms Management**: Track detailed information about each firearm in your collection
- **Activity Tracking**: Record purchases, sales, repairs, modifications, range sessions, and more
- **Inventory Systems**: Manage accessories and ammunition with full CRUD operations
- **Document Management**: Upload and organize receipts, manuals, certificates, and photos
- **Reporting**: Generate comprehensive reports on your collection
- **Multi-User Support**: Role-based access control with Owner, Administrator, PowerUser, and ReadOnly roles
- **Flexible Database Options**: Choose between SQLite (simple, file-based) or PostgreSQL (advanced, server-based)

## About This Project

I created FirearmTracker to help a family member who is a firearms enthusiast and collector. They had been using an older Windows-based commercial application for years, but that software is no longer available or supported. With their transition from a laptop to an iPad, they needed a modern, web-based solution that could be self-hosted on their local network to maintain privacy and control over their collection data.

While I'm not currently a professional developer, I have a background in IT including previous experience as a Windows developer. I built this application with significant assistance from Claude (Anthropic's AI assistant), which helped architect the solution, write much of the codebase, and establish modern best practices throughout.

I'm sharing this project openly as both a learning journey in modern web development and in hopes it might be useful to others in similar situations - whether managing their own collections, helping family members do so, or simply looking for a privacy-focused, self-hosted solution. **I enthusiastically welcome feedback, contributions, and suggestions from experienced developers.** If you see opportunities for improvement - whether in code quality, architecture, security, or features - please don't hesitate to open an issue or submit a pull request. This is very much a community project, and I'm eager to learn and improve it with your help.

While the application is functional and serves my needs well, I recognize there may be areas where professional developers would approach things differently. Your expertise and contributions are not just welcome - they're genuinely appreciated!

## Features

### Flexible Database Options
- **SQLite**: Simple file-based database, perfect for single-user or small installations
- **PostgreSQL**: Full-featured server-based database for advanced deployments
- Choose your database type during initial setup
- Separate migration paths ensure compatibility with both database types

### Firearms Management
- Complete firearm inventory with detailed specifications
- Track manufacturer, model, serial number, caliber/gauge, and type
- Ownership status filtering (Current, Sold, Transferred)
- Smart caliber/gauge selection system
- Comprehensive activity history for each firearm

### Activity Tracking
- **Acquisition**: Record initial collection items, purchases, transfers, and inheritances
- **Disposition**: Document sales with buyer information and transaction details
- **Maintenance**: Log DIY and professional repairs with costs and descriptions
- **Valuation**: Track self and professional appraisals over time
- **Usage**: Record range sessions with round counts and performance notes
- **Modifications**: Document accessories and upgrades
- **Malfunctions**: Track issues and resolutions
- **Insurance**: Maintain insurance policy information

### Inventory Management
- **Accessories**: Track optics, magazines, cases, and other accessories
- **Ammunition**: (Planned) Full ammunition inventory with consumption tracking

### Document Management
- Upload multiple file types (images, PDFs, videos)
- Automatic thumbnail generation for quick preview
- Organized by firearm or activity
- Secure document storage with size limits

### Reporting
- (Planned) Firearm history reports
- (Planned) Complete inventory exports to Excel
- (Planned) Shooting session summaries
- (Planned) Collection analytics and dashboard

### Security & Access Control
- Cookie-based authentication with BCrypt password hashing
- Four role levels: Owner, Administrator, PowerUser, ReadOnly
- Multi-user support with role-based permissions
- Secure session management

### Health Checks
- Automatic system health monitoring
- Displays warnings for missing dependencies (FFMPEG)
- Dismissible notification system
- Extensible for future health checks

## Technology Stack

### Backend
- **Framework**: ASP.NET Core 8.0
- **UI**: Blazor Server
- **Database**: SQLite or PostgreSQL with Entity Framework Core
- **Authentication**: Cookie-based authentication with BCrypt password hashing
- **Architecture**: Clean Architecture with separated Core, Data, and Web layers

### Key Dependencies
- **Microsoft.EntityFrameworkCore.Sqlite**: SQLite database provider
- **Npgsql.EntityFrameworkCore.PostgreSQL**: PostgreSQL database provider
- **BCrypt.Net-Next**: Secure password hashing
- **Docnet.Core**: PDF thumbnail generation
- **FFMpegCore**: Video processing and thumbnail generation
- **SixLabors.ImageSharp**: Image processing and thumbnail generation

### External Dependencies
- **PostgreSQL** (optional): Database server if using PostgreSQL option (tested with PostgreSQL 12+)
- **FFMPEG** (optional): Required for video thumbnail generation (health check system verifies availability)

### Architecture
- Interface-based dependency injection throughout
- Repository pattern for data access
- Service layer for business logic
- Clean separation of concerns across assemblies
- Separate migration assemblies for SQLite and PostgreSQL

## Installation and Setup

### Prerequisites
- .NET 8.0 SDK or later
- **Optional**: PostgreSQL 12+ (only if choosing PostgreSQL database option)
- **Optional**: FFMPEG (only required for video thumbnail generation)

### Quick Start

1. **Clone the repository:**
```bash
git clone https://github.com/imnorm69/FirearmTracker.git
cd FirearmTracker
```

2. **Restore dependencies:**
```bash
dotnet restore
```

3. **Run the application:**
```bash
dotnet run --project FirearmTracker.Web
```

4. **Navigate to the application** in your web browser:
   - Default URL: `https://localhost:5001` (or the port specified in your launch settings)

5. **Complete first-run setup:**
   - You'll be automatically redirected to the database configuration page
   - Choose your database type (SQLite or PostgreSQL) - see detailed instructions below
   - After database setup, create your first user account (automatically assigned Owner role)

### Database Setup

#### Option 1: SQLite (Recommended for Getting Started)

SQLite requires no additional setup - just choose it during first-run configuration:

1. On the database setup page, select **SQLite**
2. Accept the default database file path (`firearmtracker.db`) or specify a custom location
3. Click **Save Configuration**
4. **Restart the application**
5. The database will be created automatically

**Advantages of SQLite:**
- No separate database server required
- Simple file-based storage
- Perfect for single-user or small installations
- Easy backup (just copy the .db file)

#### Option 2: PostgreSQL (For Advanced Users)

PostgreSQL requires a running PostgreSQL server. Follow these steps:

**Step 1: Install PostgreSQL** (if not already installed)
- Download from [postgresql.org](https://www.postgresql.org/download/)
- Or install via package manager:
  - Ubuntu/Debian: `sudo apt-get install postgresql`
  - macOS: `brew install postgresql`
  - Windows: Use the installer from postgresql.org

**Step 2: Create the database and user**

1. Start the FirearmTracker application (it will redirect you to database setup)
2. On the database setup page, select **PostgreSQL**
3. Fill in your PostgreSQL connection details:
   - **Host**: Usually `localhost` (or your PostgreSQL server IP)
   - **Port**: Usually `5432` (PostgreSQL default)
   - **Database Name**: Enter `firearmtracker` (or your preferred name)
   - **Username**: Choose a username for the application (e.g., `firearmtracker_user`)
   - **Password**: Choose a secure password
4. Click **Show SQL Script** button
5. Copy the displayed SQL commands

**Step 3: Run the SQL commands in PostgreSQL**

Open pgAdmin or connect via psql and run these commands:
```sql
-- Connect to the 'postgres' database first, then run:
CREATE DATABASE firearmtracker;
CREATE USER firearmtracker_user WITH PASSWORD 'your_secure_password';
GRANT ALL PRIVILEGES ON DATABASE firearmtracker TO firearmtracker_user;

-- Now connect to the new database (firearmtracker) and run:
GRANT ALL ON SCHEMA public TO firearmtracker_user;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO firearmtracker_user;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO firearmtracker_user;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON TABLES TO firearmtracker_user;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON SEQUENCES TO firearmtracker_user;
```

**Using pgAdmin:**
1. In pgAdmin, expand your PostgreSQL server
2. Expand "Databases"
3. Right-click on "postgres" (the default database) → Query Tool
4. Enable **Auto commit** (checkbox in toolbar)
5. Paste and run each command separately

**Using psql command line:**
```bash
# Connect as postgres superuser
psql -U postgres

# Run the commands above
```

**Step 4: Test and save configuration**

1. Back in FirearmTracker, click **Test Connection** button
2. If successful, click **Save Configuration**
3. **Restart the application**
4. Migrations will run automatically to create the database schema

**Advantages of PostgreSQL:**
- Better performance for larger collections
- Advanced query capabilities
- Better support for concurrent users
- Industry-standard database with robust tooling

### Switching Databases Later

While the initial setup only allows choosing one database type, you can migrate from SQLite to PostgreSQL later using the planned backup/restore feature (currently in development). The backup files will be database-agnostic, allowing you to:

1. Backup your SQLite database
2. Reconfigure for PostgreSQL
3. Restore the backup to PostgreSQL

## Configuration

### Viewing Database Configuration

After initial setup, you can view your database configuration:
1. Log in as Owner or Administrator
2. Navigate to **Administration** → **Database Setup**
3. Current configuration is displayed in read-only mode

### Application Settings

Key configuration options in `appsettings.json` (these are defaults and can be adjusted):
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore.Database.Command": "Warning",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  },
  "FileUpload": {
    "MaxFileSizeBytes": 10485760,
    "AllowedExtensions": [".jpg", ".jpeg", ".png", ".pdf", ".mp4", ".avi", ".mov"],
    "StoragePath": "wwwroot/uploads"
  }
}
```

**Note**: Database connection settings are stored in `dbconfig.json` (automatically created during setup) rather than in `appsettings.json`.

### User Roles

- **Owner**: Full system access, can manage users and all data
- **Administrator**: Can manage all firearms and activities, but not user accounts
- **PowerUser**: Can create and edit firearms and activities
- **ReadOnly**: View-only access to all data

### Health Checks

The application includes a health check system that monitors:
- FFMPEG availability (for video processing)

Health warnings are displayed in a dismissible banner at the top of the page when issues are detected.

## Deployment

### Local/Self-Hosted Deployment

#### Windows
1. Publish the application:
```bash
dotnet publish -c Release -o ./publish
```

2. Configure IIS or run as a standalone Kestrel server
3. If using PostgreSQL, ensure it's accessible from the deployment machine
4. Install FFMPEG if video thumbnail generation is needed

#### Linux (Debian/Ubuntu)
1. Install prerequisites:
```bash
sudo apt-get update
sudo apt-get install dotnet-sdk-8.0
# Only if using PostgreSQL:
sudo apt-get install postgresql
# Only if using video thumbnails:
sudo apt-get install ffmpeg
```

2. Publish and deploy:
```bash
dotnet publish -c Release -o /var/www/firearmtracker
```

3. Create a systemd service for automatic startup:
```bash
sudo nano /etc/systemd/system/firearmtracker.service
```

4. Configure reverse proxy (nginx/Apache) as needed

#### Raspberry Pi
The application has been designed with Raspberry Pi deployment in mind:
- Use ARM-compatible .NET runtime
- SQLite is recommended for Raspberry Pi deployments
- Ensure sufficient storage for document uploads
- PostgreSQL can run on Pi 3B+ or newer if needed
- Consider disabling video thumbnail generation if FFMPEG performance is poor

### Production Considerations
- Use strong database credentials (especially for PostgreSQL)
- Configure HTTPS with valid SSL certificates
- Set appropriate file upload size limits
- Implement regular database backups:
  - SQLite: Copy the `.db` file regularly
  - PostgreSQL: Use `pg_dump` for backups
- Review and adjust logging levels for production
- Ensure `dbconfig.json` has appropriate file permissions (contains database credentials)

## Usage

### Getting Started

1. **Complete Initial Setup**: 
   - Choose your database type (SQLite or PostgreSQL)
   - Follow the setup instructions for your chosen database
   - Restart the application when prompted

2. **Create Your Account**: Register your first account (automatically assigned Owner role)

3. **Add Your First Firearm**: 
   - Navigate to Firearms → Add New
   - Enter firearm details (manufacturer, model, serial number, caliber/gauge, type)
   - Save to create your first entry

4. **Record Activities**:
   - From the firearm detail page, use the activity tabs (Transactions, Maintenance, Valuations, Usage, Modifications)
   - Click the add button to record new activities
   - All activities maintain a complete audit trail

5. **Upload Documents**:
   - Attach receipts, manuals, photos, or videos to any firearm or activity
   - Thumbnails are automatically generated for quick reference (requires FFMPEG for videos)

6. **Manage Accessories**:
   - Navigate to Accessories to create inventory items
   - Link accessories to specific firearms
   - Track which accessories are included when firearms are sold

7. **Database Backup/Restore**:
   - Navigate to Administration (must be logged in as owner or admin role)
   - Select Backup & Restore
   - NOTE:  Restore requires the current system and backup file to have the same schema version
   - NOTE:  Restore overwrites all existing data, including users and attached documents

### Common Workflows

**Recording a Purchase:**
1. Add new firearm with purchase details
2. Upload receipt/invoice documents
3. Record initial appraisal value
4. Link any included accessories

**Selling a Firearm:**
1. Navigate to firearm detail page
2. Go to Acquisition tab → Record Sale
3. Enter buyer information and sale details
4. Specify which accessories are included in sale
5. Firearm status automatically updates to "Sold"

**Tracking Maintenance:**
1. Record range sessions to track usage
2. Log any malfunctions encountered
3. Document repairs (DIY or professional) with costs
4. Upload photos of work performed

## Contributing

Contributions are welcome! This project is open source and available for community improvements.

### Development Setup

1. Fork the repository
2. Create a feature branch:
```bash
git checkout -b feature/your-feature-name
```

3. Make your changes following the existing code patterns
4. Test thoroughly across different scenarios
5. Commit with clear, descriptive messages
6. Push to your fork and submit a pull request

### Working with Migrations

When you modify the EF Core model (add/change entities), you need to create migrations for **both** database types:

**For SQLite:**
```bash
dotnet ef migrations add YourMigrationName --project FirearmTracker.Data.Migrations.Sqlite --startup-project FirearmTracker.Web
```

**For PostgreSQL:**
1. Create a temporary `dbconfig.json` in `FirearmTracker.Web/`:
```json
{
  "DatabaseType": 1,
  "SqliteFilePath": null,
  "PostgresConfig": {
    "Host": "localhost",
    "Port": 5432,
    "Database": "dummy",
    "Username": "dummy",
    "Password": "dummy"
  }
}
```

2. Run the migration:
```bash
dotnet ef migrations add YourMigrationName --project FirearmTracker.Data.Migrations.Postgres --startup-project FirearmTracker.Web
```

3. Delete the temporary `dbconfig.json`

4. Update AppVersion.cs in FirearmTracker.Core with an incremented version (db schema version) number.

### Code Standards

- Follow existing architectural patterns (interface-based services, repository pattern)
- Maintain consistency in naming conventions
- Ensure all services have corresponding interfaces in `FirearmTracker.Core.Interfaces`
- Add appropriate logging for new features
- Include XML documentation for public methods
- Test entity framework migrations for both SQLite and PostgreSQL before committing
- Keep migration projects in sync when modifying the data model

### Project Structure
```
FirearmTracker/
├── FirearmTracker.Core/                # Domain models and interfaces
├── FirearmTracker.Data/                # Repository implementations and EF context
├── FirearmTracker.Data.Migrations.Sqlite/    # SQLite-specific migrations
├── FirearmTracker.Data.Migrations.Postgres/  # PostgreSQL-specific migrations
├── FirearmTracker.Web/                 # Blazor UI, services, and pages
│   ├── Components/                    # Reusable Blazor components
│   │   ├── Admin/                    # Admin-specific components
│   │   └── Pages/                    # Page components
│   ├── Services/                      # Business logic services
│   └── wwwroot/                       # Static files and uploads
└── dbconfig.json                       # Database configuration (created on first run)
```

### Reporting Issues

When reporting bugs, please include:
- Steps to reproduce
- Expected vs actual behavior
- Database type (SQLite or PostgreSQL)
- Environment details (OS, .NET version, database version if PostgreSQL)
- Any relevant error messages or logs

### Feature Requests

Check the [User Stories document](docs/backlog.md) to see planned features. New ideas are welcome - open an issue for discussion before starting major work.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Built with [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet) and [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
- Icons and UI components from custom SVG icon system
- PDF processing by [Docnet.Core](https://github.com/GowenGit/docnet)
- Video processing by [FFMpegCore](https://github.com/rosenbjerg/FFMpegCore)
- Image processing by [ImageSharp](https://github.com/SixLabors/ImageSharp)
- Database support: SQLite and PostgreSQL via Entity Framework Core

## Support

For questions, issues, or suggestions:
- Open an issue on GitHub
- Check existing issues and documentation first
- Provide detailed information for bug reports

## Security

If you discover a security vulnerability, please email [your-email@example.com] instead of using the issue tracker.

## Disclaimer

This application is designed for personal collection management only. Users are responsible for complying with all applicable local, state, and federal laws regarding firearm ownership, transfers, and record-keeping. The authors and contributors assume no liability for misuse of this software.

---

**Project Status**: Active Development | **Current Version**: 0.6.0 (Pre-Release)

**Recent Updates (v0.6.0)**:
- Added flexible database configuration (SQLite or PostgreSQL)
- Implemented health check system with FFMPEG detection
- Separated migration assemblies for better maintainability
- Enhanced first-run setup experience