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

## About This Project

## About This Project

I created FirearmTracker to help a family member who is a firearms enthusiast and collector. They had been using an older Windows-based commercial application for years, but that software is no longer available or supported. With their transition from a laptop to an iPad, they needed a modern, web-based solution that could be self-hosted on their local network to maintain privacy and control over their collection data.

While I'm not currently a professional developer, I have a background in IT including previous experience as a Windows developer. I built this application with significant assistance from Claude (Anthropic's AI assistant), which helped architect the solution, write much of the codebase, and establish modern best practices throughout.

I'm sharing this project openly as both a learning journey in modern web development and in hopes it might be useful to others in similar situations - whether managing their own collections, helping family members do so, or simply looking for a privacy-focused, self-hosted solution. **I enthusiastically welcome feedback, contributions, and suggestions from experienced developers.** If you see opportunities for improvement - whether in code quality, architecture, security, or features - please don't hesitate to open an issue or submit a pull request. This is very much a community project, and I'm eager to learn and improve it with your help.

While the application is functional and serves my needs well, I recognize there may be areas where professional developers would approach things differently. Your expertise and contributions are not just welcome - they're genuinely appreciated!

## Features

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

## Technology Stack

### Backend
- **Framework**: ASP.NET Core 8.0
- **UI**: Blazor Server
- **Database**: PostgreSQL with Entity Framework Core
- **Authentication**: Cookie-based authentication with BCrypt password hashing
- **Architecture**: Clean Architecture with separated Core, Data, and Web layers

### Key Dependencies
- **Npgsql.EntityFrameworkCore.PostgreSQL**: PostgreSQL database provider
- **BCrypt.Net-Next**: Secure password hashing
- **Docnet.Core**: PDF thumbnail generation
- **FFMpegCore**: Video processing and thumbnail generation
- **SixLabors.ImageSharp**: Image processing and thumbnail generation

### External Dependencies
- **PostgreSQL**: Database server (tested with PostgreSQL 12+)
- **FFMPEG**: Required for video thumbnail generation (health check system verifies availability)

### Architecture
- Interface-based dependency injection throughout
- Repository pattern for data access
- Service layer for business logic
- Clean separation of concerns across assemblies

## Installation and Setup

### Prerequisites
- .NET 8.0 SDK or later
- PostgreSQL 12+ installed and running
- FFMPEG (optional, required only for video thumbnail generation)

### Database Setup

1. Create a PostgreSQL database:
```sql
CREATE DATABASE firearmtracker;
```

2. Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=firearmtracker;Username=your_username;Password=your_password"
  }
}
```

3. Apply database migrations:
```bash
dotnet ef database update
```

### First Run

1. Clone the repository:
```bash
git clone https://github.com/yourusername/FirearmTracker.git
cd FirearmTracker
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Run the application:
```bash
dotnet run --project FirearmTracker.Web
```

4. Navigate to `https://localhost:5001` (or the port specified in your launch settings)

5. Create your first user account (the first user created will automatically be assigned the Owner role)

## Configuration

### Application Settings

Key configuration options in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=firearmtracker;Username=your_username;Password=your_password"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
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

### User Roles

- **Owner**: Full system access, can manage users and all data
- **Administrator**: Can manage all firearms and activities, but not user accounts
- **PowerUser**: Can create and edit firearms and activities
- **ReadOnly**: View-only access to all data

### Health Checks

The application includes a health check system that monitors:
- Database connectivity
- FFMPEG availability (for video processing)

Health warnings are displayed in a dismissible banner when issues are detected.

## Deployment

### Local/Self-Hosted Deployment

#### Windows
1. Publish the application:
```bash
dotnet publish -c Release -o ./publish
```

2. Configure IIS or run as a standalone Kestrel server
3. Ensure PostgreSQL is accessible from the deployment machine
4. Install FFMPEG if video thumbnail generation is needed

#### Linux (Debian/Ubuntu)
1. Install prerequisites:
```bash
sudo apt-get update
sudo apt-get install dotnet-sdk-8.0 postgresql ffmpeg
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
- Ensure sufficient storage for document uploads
- PostgreSQL runs well on Pi 3B+ or newer
- Consider disabling video thumbnail generation if FFMPEG performance is poor

### Production Considerations
- Use strong database credentials
- Configure HTTPS with valid SSL certificates
- Set appropriate file upload size limits
- Implement regular database backups
- Review and adjust logging levels for production

## Usage

### Getting Started

1. **Create Your Account**: On first launch, register an account (automatically assigned Owner role)

2. **Add Your First Firearm**: 
   - Navigate to Firearms → Add New
   - Enter firearm details (manufacturer, model, serial number, caliber/gauge, type)
   - Save to create your first entry

3. **Record Activities**:
   - From the firearm detail page, use the activity tabs (Transactions, Maintenance, Valuations, Usage, Modifications)
   - Click the add button to record new activities
   - All activities maintain a complete audit trail

4. **Upload Documents**:
   - Attach receipts, manuals, photos, or videos to any firearm or activity
   - Thumbnails are automatically generated for quick reference

5. **Manage Accessories**:
   - Navigate to Accessories to create inventory items
   - Link accessories to specific firearms
   - Track which accessories are included when firearms are sold

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

### Code Standards

- Follow existing architectural patterns (interface-based services, repository pattern)
- Maintain consistency in naming conventions
- Ensure all services have corresponding interfaces
- Add appropriate logging for new features
- Include XML documentation for public methods
- Test entity framework migrations before committing

### Project Structure
```
FirearmTracker/
├── FirearmTracker.Core/       # Domain models and interfaces
├── FirearmTracker.Data/        # Repository implementations and EF context
├── FirearmTracker.Web/         # Blazor UI, services, and pages
│   ├── Components/            # Reusable Blazor components
│   ├── Pages/                 # Page components
│   ├── Services/              # Business logic services
│   └── wwwroot/               # Static files and uploads
└── Migrations/                # Entity Framework migrations
```

### Reporting Issues

When reporting bugs, please include:
- Steps to reproduce
- Expected vs actual behavior
- Environment details (OS, .NET version, database version)
- Any relevant error messages or logs

### Feature Requests

Check the [User Stories document](Firearm_Tracker_-_All_User_Stories.md) to see planned features. New ideas are welcome - open an issue for discussion before starting major work.

## Roadmap

### High Priority (Next Release)
- **Ammunition Inventory System**: Full ammunition tracking with caliber linking and consumption tracking
- **Reporting Suite**: 
  - Firearm history reports
  - Complete inventory Excel export
  - Shooting session summaries
- **Document Enhancements**: Video upload support with previews

### Medium Priority
- **UI Improvements**: 
  - Inline image preview
  - Mobile/tablet responsive layouts
  - Document upload UI refinements
- Collection dashboard with summary statistics

### Future Considerations
- Comprehensive validation framework
- Data backup and restore system
- Multi-tenant architecture for shared hosting
- Advanced search and filtering
- API for third-party integrations
- Mobile applications (iOS/Android)

See the complete [User Stories document](Firearm_Tracker_-_All_User_Stories.md) for detailed feature tracking.

### Current Status
- **Completion Rate**: 46% (24 of 52 user stories completed)
- **Core Features**: Complete and functional
- **Active Development**: Inventory and reporting modules


## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Built with [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet) and [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
- Icons and UI components from custom SVG icon system
- PDF processing by [Docnet.Core](https://github.com/GowenGit/docnet)
- Video processing by [FFMpegCore](https://github.com/rosenbjerg/FFMpegCore)
- Image processing by [ImageSharp](https://github.com/SixLabors/ImageSharp)

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

**Project Status**: Active Development | **Current Version**: 0.5.0 (Pre-Release)

