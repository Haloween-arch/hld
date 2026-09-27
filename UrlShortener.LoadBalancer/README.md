# URL Shortener Load Balancer

This project uses YARP to route traffic to one or more backend API instances. It acts as the entry point for the URL shortener service and can distribute requests across multiple nodes.

## Purpose

- serve as a single front door for the app
- distribute traffic using round-robin balancing
- perform health checks against backend instances
- help demonstrate a simple HLD / load-balancing pattern

## Key technology

- YARP Reverse Proxy
- configuration-based routing in the `ReverseProxy` section
- health checks enabled for backend destinations

## Configuration

The proxy configuration is stored in appsettings.json and includes:

- route definitions
- cluster definitions
- destination addresses for each API instance
- health check settings

## Run locally

```powershell
cd UrlShortener.LoadBalancer
dotnet run --urls "http://localhost:5000"
```

The default cluster points to:

- http://localhost:5075/
- http://localhost:5076/

## Example request flow

```text
Client -> http://localhost:5000 -> YARP -> API Instance 1 or 2
```

The backend sets the `X-Api-Instance` header so you can confirm which API handled the request.

## Important note

Keep appsettings files local and uncommitted, because they may contain routing and connection secrets.
