using Microsoft.AspNetCore.Mvc;
using UrlShortener.DTOs;
using UrlShortener.Services;

namespace UrlShortener.Controllers;

[ApiController]
[Route("api/urls")]
public class UrlController : ControllerBase
{
    private readonly IUrlService _urlService;

    public UrlController(IUrlService urlService)
    {
        _urlService = urlService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUrl(
        [FromBody] CreateUrlRequest request)
    {
        var url = await _urlService.CreateUrlAsync(request.Url);

        var response = new UrlResponse
        {
            ShortCode = url.ShortCode,
            ShortUrl = $"{Request.Scheme}://{Request.Host}/{url.ShortCode}"
        };

        return Ok(response);
    }

    [HttpGet("{shortCode}")]
    public async Task<IActionResult> RedirectToOriginalUrl(
        string shortCode)
    {
        var originalUrl =
            await _urlService.GetOriginalUrlAsync(shortCode);

        if (originalUrl == null)
            return NotFound();

        return Redirect(originalUrl);
    }

    [HttpGet("{shortCode}/stats")]
    public async Task<IActionResult> GetStats(
        string shortCode)
    {
        var url = await _urlService.GetStatsAsync(shortCode);

        if (url == null)
            return NotFound();

        return Ok(url);
    }
}