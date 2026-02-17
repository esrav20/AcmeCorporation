# Acme Corporation – Prize Draw

An ASP.NET web application for Acme Corporation's prize draw campaign. 
Customers can enter the draw twice using valid product serial numbers.

Built with .NET 10, EF Core, and SQL Server.

## Stack
- .NET 10 SDK
- SQL Server (local instance)
- EF Core CLI

## Project Structure

- **AcmeCorporation.Web** - ASP.NET MVC web application.
  - ViewModels 
  - Controllers
  - Views
  - Services
  - Depency registration 
- **AcmeCorporation.Core** - Class library
  - Models
  - Service interfaces
  - Validators
- **AcmeCorporation.Tests**
  - Unit tests

## Getting Started

### 1. Database

Create a local SQL Server database:
```sql
CREATE DATABASE AcmeCorporationDb;
```

Then update the connection string in `AcmeCorporation.Web/appsettings.json` to match your SQL Server instance:
```json
"ConnectionStrings": {
	    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=AcmeCorporationDb;Trusted_Connection=True;TrustServerCertificate=True"
```

### 2. Build & Test

```bash
# Clone repo

git clone https://github.com/esrav20/AcmeCorporation.git

cd AcmeCorporation

# Build project
dotnet build

# Run tests
dotnet test
```

### 3. Run 
```bash
cd AcmeCorporation.Web

dotnet ef database update

# Run web-app
dotnet run

# App will be available on port 5265
http://localhost:5265
```

On first run, 100 serial numbers are seeded into the database and exported to `SerialNumbers.txt`.
