using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApp.Server.Attributes;
using WebApp.Server.Consts;
using WebApp.Server.Interfaces;
using WebApp.Server.Models;

namespace WebApp.Server.Controllers;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class LinkController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<LinkController> _logger;

    public LinkController(IOptions<APIServer> server, IHttpClientHelper httpClient, ILogger<LinkController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        _httpClient.APIUrl = server.Value.Document;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LinkModel item)
    {
        var response = await _httpClient.PostAsync("Link", JsonContent.Create(item));

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Link {LinkId} created successfully for UserId: {ToUserId}", item.Id, item.ToUserId);

            return Ok();
        }

        if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            _logger.LogWarning("Downstream API returned problem for Link {LinkId}: {Title} - {Detail}", item.Id, problem?.Title, problem?.Detail);

            return StatusCode((int)response.StatusCode, problem);
        }

        _logger.LogError("Unexpected response from Comment API for Link {LinkId}. Status: {StatusCode}", item.Id, response.StatusCode);

        return StatusCode((int)response.StatusCode);

    }

    [HttpGet("getByUserId/{id}")]
    public async Task<IActionResult> GetByUserId(Guid id, [FromQuery] int page, [FromQuery] int pageSize)
    {
        var response = await _httpClient.GetAsync($"Link/getByUserId/{id}?page={page}&pageSize={pageSize}");

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Link by Document {DocumentId} extracted successfully.", id);

            var links = await response.Content.ReadFromJsonAsync<IEnumerable<LinkModel>>();

            return Ok(links);
        }

        if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            _logger.LogWarning("Downstream API returned problem for Document {DocumentId}: {Title} - {Detail}", id, problem?.Title, problem?.Detail);

            return StatusCode((int)response.StatusCode, problem);
        }

        _logger.LogError("Unexpected response from Comment API for Document {DocumentId}. Status: {StatusCode}", id, response.StatusCode);

        return StatusCode((int)response.StatusCode);
    }

    [HttpDelete("documents/{documentId}/links/{linkId}")]
    public async Task<IActionResult> Delete(Guid documentId, Guid linkId)
    {
        var response = await _httpClient.DeletAsync($"Link/documents/{documentId}/links/{linkId}");

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Link {LinkId} deleted successfully.", linkId);

            return NoContent();
        }

        if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            _logger.LogWarning("Downstream API returned problem for Link {LinkId}: {Title} - {Detail}", linkId, problem?.Title, problem?.Detail);

            return StatusCode((int)response.StatusCode, problem);
        }

        _logger.LogError("Unexpected response from Comment API for Link {LinkId}. Status: {StatusCode}", linkId, response.StatusCode);

        return StatusCode((int)response.StatusCode);
    }
}
