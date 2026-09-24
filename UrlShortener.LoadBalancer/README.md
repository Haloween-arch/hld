# URL Shortener Load Balancer

This project is a lightweight reverse proxy built with YARP. It sits in front of the URL shortener API instances and forwards incoming traffic to the backend service.

## Purpose

- centralize public entry point
- distribute traffic across multiple API instances
- allow testing of failover or instance-aware routing behavior

## Key Technology

- YARP Reverse Proxy
- configuration-driven routing under `ReverseProxy`

## Run

```powershell
cd UrlShortener.LoadBalancer
dotnet run --urls "http://localhost:5000"
```

The proxy is configured through appsettings and related configuration sections, which should remain local to your environment and not be committed to source control.

## Typical Setup

Run multiple API instances behind the load balancer, then send requests through the proxy endpoint. The backend response header `X-Api-Instance` helps confirm which instance processed the request.
