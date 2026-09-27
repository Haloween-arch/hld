# URL Shortener API

This project is the backend service for the short URL application. It stores URLs in PostgreSQL and exposes API endpoints for creating, resolving, and updating short links.

## Responsibilities

- create short URLs from long URLs
- generate unique short codes using Base62 logic
- persist data in PostgreSQL
- redirect short codes to the original URL
- expose service health and metadata endpoints
- return which API instance processed the request through the `X-Api-Instance` response header

## Main components

- Controllers/UrlController.cs — API endpoints
- Services/UrlService.cs — business logic
- Repositories/UrlRepository.cs — data access
- Data/AppDbContext.cs — EF Core database context
- Models/Url.cs — URL entity
- Utils/Base62Generator.cs — short code generation
- DTOs/ — request and response models

## API endpoints

```text
POST   /api/urls
GET    /api/urls/{shortCode}
GET    /api/urls/{shortCode}/stats
PUT    /api/urls/{shortCode}
GET    /health
```

## How it works

1. A client sends a long URL to `POST /api/urls`.
2. The service generates a short code and stores it in PostgreSQL.
3. The API returns a short URL like `http://localhost:5075/{shortCode}`.
4. The redirect endpoint resolves the short code and sends the user to the original destination.
5. For faster reads, the service tries Redis first using a cache-aside pattern.

## Caching strategy

This service implements cache-aside caching with Redis:

- The key format is `url:{shortCode}`.
- Redis is checked before hitting PostgreSQL.
- If the code is cached, the original URL is returned immediately.
- If not found, the URL is loaded from the database, cached for future requests, and then returned.
- When a URL is updated, the cache key is deleted to invalidate stale data.

This ensures the application remains fast for repeated short-link lookups while still keeping the source of truth in PostgreSQL.

## Run locally

```powershell
cd URL_shortner
$env:API_INSTANCE="API-1"
dotnet run --urls "http://localhost:5075"
```

You can also start a second instance:

```powershell
cd URL_shortner
$env:API_INSTANCE="API-2"
dotnet run --urls "http://localhost:5076"
```

## Configuration

The project reads connection settings from appsettings JSON files. These files may contain real database credentials and must not be committed to Git. Use local-only config files or environment variables for secrets.
