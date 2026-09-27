# HLD URL Shortener

This workspace contains a small URL shortener system built with ASP.NET Core, PostgreSQL, and YARP reverse proxying.

## Overview

The solution demonstrates a basic load-balanced API setup:

- The API project handles creating, reading, updating, and redirecting short URLs.
- The load balancer project sits in front of the API and distributes requests across multiple API instances.
- Each API instance can identify itself with an `X-Api-Instance` response header.
- Redis is used as a read-through / cache-aside layer for faster URL lookups.

## Projects

### 1. URL_shortner
This is the main backend service. It stores URLs and redirects short codes to the original domain.

Key responsibilities:

- create short URLs
- resolve short codes
- update an existing URL mapping
- return metadata for a short code
- expose health endpoints

### 2. UrlShortener.LoadBalancer
This is a YARP-based load balancer. It routes incoming traffic to the configured API destinations using round-robin behavior and health checks.

## Architecture

```text
Client
  |
  v
Load Balancer (YARP)
  |
  +--> API Instance 1 (localhost:5075)
  |
  +--> API Instance 2 (localhost:5076)
```

## Run the project

Start the load balancer first:

```powershell
cd UrlShortener.LoadBalancer
dotnet run --urls "http://localhost:5000"
```

Then start one or more API instances:

```powershell
cd ../URL_shortner
$env:API_INSTANCE="API-1"
dotnet run --urls "http://localhost:5075"
```

```powershell
cd ../URL_shortner
$env:API_INSTANCE="API-2"
dotnet run --urls "http://localhost:5076"
```

## Example requests

Create a short URL:

```http
POST http://localhost:5000/api/urls
Content-Type: application/json

{
  "url": "https://example.com"
}
```

Resolve a short URL:

```http
GET http://localhost:5000/LdOk3e5pZ2
```

## API endpoints

In the API project, the controller exposes:

- `POST /api/urls`
- `GET /api/urls/{shortCode}`
- `GET /api/urls/{shortCode}/stats`
- `PUT /api/urls/{shortCode}`

## Caching strategy

The API uses a cache-aside pattern with Redis:

- On a lookup, the app checks Redis first using a key like `url:{shortCode}`.
- If the key exists, it is returned immediately as a cache hit.
- If the key is missing, the app reads from PostgreSQL, stores the result in Redis, and then returns it.
- When a URL is updated, the matching Redis key is removed so the next request fetches the fresh value from the database.

This keeps hot URLs fast while ensuring stale entries are invalidated after updates.

## Configuration and security

This project uses configuration files such as appsettings.json. These may contain database credentials, Redis connection strings, and other secrets, so they are intentionally excluded from Git by the root [.gitignore](.gitignore).

Keep local secrets in non-committed config files or environment variables only.

## Notes

- The app uses PostgreSQL via Entity Framework Core.
- The load balancer is configured in the `ReverseProxy` section of the appsettings file.
- This is a demo-style HLD project and can be expanded with production features such as caching, monitoring, failover, and authentication.
