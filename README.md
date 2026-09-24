# HLD URL Shortener

This workspace contains a simple URL shortener system with two ASP.NET Core services:

- URL_shortner: the backend API that creates and resolves short URLs.
- UrlShortener.LoadBalancer: a reverse proxy that distributes traffic across backend instances.

## Architecture

The system is designed to demonstrate a basic high-availability pattern:

- The API service stores shortened links in PostgreSQL.
- The load balancer sits in front of one or more API instances.
- Requests are routed through the reverse proxy, which can be used to spread traffic across multiple backend nodes.
- Each backend can expose an `X-Api-Instance` response header to confirm which instance handled the request.

## Projects

### 1. URL_shortner
This service handles URL creation and redirect logic. It exposes API endpoints for:

- creating a short URL
- resolving a short code to the original URL
- health checks

### 2. UrlShortener.LoadBalancer
This is a YARP-based reverse proxy. It forwards incoming requests to the configured API backend(s) and is useful for demonstrating request routing and instance balancing.

## Typical Run Flow

1. Start the load balancer.
2. Start one or more API instances with different `API_INSTANCE` values.
3. Send requests through the load balancer.
4. Observe which API instance handles the request from the response header.

## Example

```powershell
# Load balancer
cd UrlShortener.LoadBalancer
dotnet run --urls "http://localhost:5000"

# API 1
cd ../URL_shortner
$env:API_INSTANCE="API-1"
dotnet run --urls "http://localhost:5075"

# API 2
cd ../URL_shortner
$env:API_INSTANCE="API-2"
dotnet run --urls "http://localhost:5076"
```

## Notes

- Configuration files such as appsettings JSON files are ignored by Git for local secret protection.
- This project is intended as a learning/demo setup and can be extended with production-grade load balancing, persistence, and observability.
