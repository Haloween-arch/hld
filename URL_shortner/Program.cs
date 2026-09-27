using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Repositories;
using UrlShortener.Services;
using StackExchange.Redis;

var apiInstance =
    Environment.GetEnvironmentVariable("API_INSTANCE")
    ?? "unknown";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHealthChecks();


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});

builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(
        builder.Configuration["Redis:ConnectionString"]!
    )
);

builder.Services.AddScoped<IUrlRepository, UrlRepository>();
builder.Services.AddScoped<IUrlService, UrlService>();
builder.Services.AddSingleton<ICacheService, CacheService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    context.Response.Headers["X-Api-Instance"] = apiInstance;
    await next();
});

app.UseHttpsRedirection();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();