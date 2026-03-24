# Guest Registration App

A web application for event guest registration with an admin dashboard for managing entries.
The live app can be accessed at https://guestregistration.azurewebsites.net or with the QR code below.

<img src="images/F71GRB.jpg" alt="QR code" width="200">


## Features

- **Guest Registration Form** - Collect name, email, and phone number
- **Admin Dashboard** - Passcode-protected access to view all registrations
- **Azure SQL Database** - Cloud-hosted database for storing registrations
- **Responsive Design** - Works on desktop and mobile devices

## Tech Stack

| Component | Technology |
|-----------|------------|
| Framework | ASP.NET Core 10 (Razor Pages) |
| Database | Azure SQL Database |
| Driver | Microsoft.Data.SqlClient |
| Frontend | Bootstrap 5, jQuery |
| Authentication | Session-based |
| Hosting | Azure App Service |

## Project Structure

```
GuestRegistration/
├── Program.cs              # API endpoints and app configuration
├── GuestRegistration.csproj # Project file
├── appsettings.json        # Configuration (connection string in Azure)
├── Models/
│   └── Guest.cs            # Guest data model
├── Pages/
│   ├── Index.cshtml        # Registration form
│   ├── Admin.cshtml        # Admin dashboard
│   ├── Success.cshtml      # Success confirmation
│   └── Shared/_Layout.cshtml # Main layout
└── wwwroot/
    ├── css/                # Styles
    └── lib/                # Bootstrap, jQuery
```

## Setup

### Prerequisites

- .NET 10 SDK
- Azure subscription (or Azure for Students)
- Azure SQL Database`

## Deployment

### Azure App Service

1. Create an Azure App Service with .NET 10 runtime

2. Set application settings in Azure Portal:
   - `DefaultConnection` - Your Azure SQL connection string
   - `AdminPasscode` - Passcode for admin access

3. Deploy using one of these methods:

   **VS Code:**
   - Install Azure App Service extension
   - Right-click project → Deploy to Web App

   **Azure CLI:**
   ```bash
   az webapp up --name your-app-name --runtime "DOTNET|10.0"
   ```

   **ZIP Deploy:**
   ```bash
   dotnet publish -c Release
   az webapp deployment source config-zip \
     --resource-group your-resource-group \
     --name your-app-name \
     --src ./bin/Release/net10.0/publish/GuestRegistration.zip
   ```

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/guests` | Register a new guest |
| GET | `/api/guests` | Get all guests (JSON) |

### POST /api/guests

**Request:**
```json
{
  "fullName": "John Doe",
  "email": "john@example.com",
  "phoneNumber": 1234567890
}
```

**Response:**
```json
{
  "id": 1,
  "fullName": "John Doe",
  "email": "john@example.com",
  "phoneNumber": 1234567890
}
```

## Pages

| Page | URL | Description |
|------|-----|-------------|
| Registration | `/` | Guest registration form |
| Admin | `/Admin` | Protected dashboard (requires passcode) |
| Success | `/Success` | Registration success confirmation |

## Security

- Credentials stored in Azure Environment Variables (not in code)
- Session-based authentication for admin access
- HTTPS enforced in production
- `.gitignore` excludes sensitive files
