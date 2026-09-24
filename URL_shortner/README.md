# URL Shortener API

This project is the backend service for the short URL application. It stores URLs in PostgreSQL and exposes HTTP endpoints for creating and resolving short links.

## Responsibilities

- create short URLs from long URLs
- generate short codes using a Base62 strategy
- persist metadata in the database
- redirect short links back to their original destination
- report the instance name in the response header

## Main Components

- Controllers/UrlController.cs: API entry points
- Services/UrlService.cs: business logic
- Repositories/UrlRepository.cs: persistence logic
- Data/AppDbContext.cs: EF Core database context
- Models/Url.cs: domain model
- Utils/Base62Generator.cs: code generation helper

## Example Endpoints

- POST /api/urls
- GET /api/urls/{shortCode}
- GET /health

## Configuration

The app reads database settings from configuration files such as appsettings.json and any environment-specific overrides. For local development, keep secrets out of Git using the root .gitignore rules.

## Run

```powershell
cd URL_shortner
dotnet run --urls "http://localhost:5075"
```

You can set an instance label before starting:

```powershell
$env:API_INSTANCE="API-1"
dotnet run --urls "http://localhost:5075"
```

This value is returned in the `X-Api-Instance` response header.
