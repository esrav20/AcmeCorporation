# Acme Corporation – Prize Draw

## Prerequisites

- .NET 10 SDK
- SQL Server installed locally
- EF Core CLI

## Clone Repository

```bash
# Clone repo
git clone https://github.com/esrav20/AcmeCorporation.git

cd AcmeCorporation

# Build project
dotnet build

# Go to Web app to update the database and run the site
cd AcmeCorporation.Web

dotnet ef database update

cd AcmeCorporation
# Run tests
dotnet test

# Run web-app
dotnet run

# App should be running on port 5265
http://localhost:5265
```

## Database Setup

Create a local SQL Server database before running any migrations.

Mine is hosted on localhost\SQLEXPRESS, but adjust *appsettings.json* to match your SQL Server path after creating database

```sql
CREATE DATABASE AcmeCorporationDb;
```

```java
"ConnectionStrings": {
	    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=AcmeCorporationDb;Trusted_Connection=True;TrustServerCertificate=True"
```

---

On first run, 100 serial numbers are seeded into the database and exported to `SerialNumbers.txt`.
