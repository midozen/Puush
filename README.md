# Puush API

ASP.NET Core Web Application that aims to revive the original puush image hosting service (circa 2010).

## Prerequisites

- .NET SDK 10.0.x
- MySQL 8+ (or compatible)
- Cloudflare R2 bucket + API credentials

## 1. Restore Dependencies

From repo root:

```powershell
dotnet restore Puush.sln
```

## 2. Configure Secrets (Required)

This project uses ASP.NET Core User Secrets on the web project (`Puush/Puush.csproj`).

From repo root, set these values:

```powershell
dotnet user-secrets set -p Puush "ConnectionStrings:DefaultConnection" "server=localhost;port=3306;database=puush;user=YOUR_USER;password=YOUR_PASSWORD"
dotnet user-secrets set -p Puush "R2:AccountId" "YOUR_R2_ACCOUNT_ID"
dotnet user-secrets set -p Puush "R2:AccessKey" "YOUR_R2_ACCESS_KEY"
dotnet user-secrets set -p Puush "R2:SecretKey" "YOUR_R2_SECRET_KEY"
dotnet user-secrets set -p Puush "R2:BucketName" "YOUR_BUCKET_NAME"
dotnet user-secrets set -p Puush "Puush:BaseUrl" "http://localhost:5168"
```

Optional: inspect what you set:

```powershell
dotnet user-secrets list -p Puush
```

## 3. Apply Database Migrations

```powershell
dotnet ef database update -p Puush.Persistence -s Puush
```

If `dotnet ef` is not installed:

```powershell
dotnet tool install --global dotnet-ef
```

## 4. Run the API

```powershell
dotnet run -p Puush
```

## Notes

- `ConnectionStrings:DefaultConnection` is required at startup.
- All `R2:*` secrets above are required for upload/thumbnail/delete operations.
- `Puush:BaseUrl` is used when building file URLs returned by API responses.
