# Guest Registration App

A web application for event guest registration with an admin dashboard for managing entries.
The live app can be accessed at https://guestregistration.azurewebsites.net or with the QR code below.

<img src="https://github.com/sara-johari/GuestRegistration/images/F71GRB.jpg" height="250">


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
- Azure subscription
- Azure SQL Database

### Local Development

1. Clone the repository:
   ```bash
   git clone https://github.com/YOUR-USERNAME/GuestRegistration.git
   cd GuestRegistration
   ```

2. Add connection string to `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=...;Database=...;User Id=...;Password=..."
     }
   }
   ```

3. Create the `guests` table in your SQL database:
   ```sql
   CREATE TABLE guests (
       id INT IDENTITY(1,1) PRIMARY KEY,
       full_name VARCHAR(255) NOT NULL,
       email VARCHAR(255) NOT NULL,
       phone INT NOT NULL
   );
   ```

4. Run the app:
   ```bash
   dotnet run
   ```

5. Navigate to `http://localhost:5213`

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

- Credentials stored in Azure Application Settings (not in code)
- Session-based authentication for admin access
- HTTPS enforced in production
- `.gitignore` excludes sensitive files

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Open a Pull Request
