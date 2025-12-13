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
public class DocumentController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<DocumentController> _logger;

    public DocumentController(IOptions<APIServer> server, IHttpClientHelper httpClient, ILogger<DocumentController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        _httpClient.APIUrl = server.Value.Document;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DocumentModel item)
    {
        var response = await _httpClient.PostAsync("Document", JsonContent.Create(item));

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Document {DocumentId} created successfully for UserId: {UserId}", item.Id, item.UserId);
            return Ok();
        }

        if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            _logger.LogWarning("Downstream API returned problem for Document {DocumentId}: {Title} - {Detail}", item.Id, problem?.Title, problem?.Detail);

            return StatusCode((int)response.StatusCode, problem);
        }

        _logger.LogError("Unexpected response from Document API for Document {DocumentId}. Status: {StatusCode}", item.Id, response.StatusCode);

        return StatusCode((int)response.StatusCode);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _httpClient.GetAsync($"Document/{id}");

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Document {DcoumentId} extracted successfully.", id);

            var document = await response.Content.ReadFromJsonAsync<DocumentModel>();

            return Ok(document);
        }

        if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            _logger.LogWarning("Downstream API returned problem for Document {DcoumentId}: {Title} - {Detail}", id, problem?.Title, problem?.Detail);

            return StatusCode((int)response.StatusCode, problem);
        }

        _logger.LogError("Unexpected response from Document API for Documents {DcoumentId}. Status: {StatusCode}", id, response.StatusCode);

        return StatusCode((int)response.StatusCode);
    }

    [HttpGet("getByName/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var response = await _httpClient.GetAsync($"Document/getByName/{name}");

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Document {DcoumentName} extracted successfully.", name);

            var document = await response.Content.ReadFromJsonAsync<DocumentModel>();

            return Ok(document);
        }

        if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            _logger.LogWarning("Downstream API returned problem for Document {DcoumentName}: {Title} - {Detail}", name, problem?.Title, problem?.Detail);

            return StatusCode((int)response.StatusCode, problem);
        }

        _logger.LogError("Unexpected response from Document API for Documents {DcoumentName}. Status: {StatusCode}", name, response.StatusCode);

        return StatusCode((int)response.StatusCode);
    }

    [HttpGet("getActualByUserId/{id}")]
    public async Task<IActionResult> GetActualByUserId(Guid id, [FromQuery] int page, [FromQuery] int pageSize)
    {
        var response = await _httpClient.GetAsync($"Document/getActualByUserId/{id}?page={page}&pageSize={pageSize}");

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Document {DocumentId} extracted successfully.", id);

            var documents = await response.Content.ReadFromJsonAsync<IEnumerable<DocumentModel>>();

            return Ok(documents);
        }

        if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            _logger.LogWarning("Downstream API returned problem for Document {DocumentId}: {Title} - {Detail}", id, problem?.Title, problem?.Detail);

            return StatusCode((int)response.StatusCode, problem);
        }

        _logger.LogError("Unexpected response from Document API for Document {DocumentId}. Status: {StatusCode}", id, response.StatusCode);

        return StatusCode((int)response.StatusCode);
    }

    [HttpGet("getHustoryByUserId/{id}")]
    public async Task<IActionResult> GetHustoryByUserId(Guid id, [FromQuery] int page, [FromQuery] int pageSize)
    {
        var response = await _httpClient.GetAsync($"Document/getHustoryByUserId/{id}?page={page}&pageSize={pageSize}");

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Document {DcoumentId} extracted successfully.", id);

            var documents = await response.Content.ReadFromJsonAsync<IEnumerable<DocumentModel>>();

            return Ok(documents);
        }

        if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            _logger.LogWarning("Downstream API returned problem for Document {DcoumentId}: {Title} - {Detail}", id, problem?.Title, problem?.Detail);

            return StatusCode((int)response.StatusCode, problem);
        }

        _logger.LogError("Unexpected response from Document API for Document {DocumentId}. Status: {StatusCode}", id, response.StatusCode);

        return StatusCode((int)response.StatusCode);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PartialUpdate(Guid id, [FromBody] DocumentModel item)
    {
        if (id != item.Id)
        {
            _logger.LogWarning("Route ID and body ID do not match.");

            return BadRequest("Route ID and body ID do not match.");
        }

        var response = await _httpClient.PatchAsync($"Document/{id}", JsonContent.Create(item));

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Document {DocumentId} updated successfully.", id);

            return NoContent();
        }

        if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            _logger.LogWarning("Downstream API returned problem for Document {DocumentId}: {Title} - {Detail}", id, problem?.Title, problem?.Detail);

            return StatusCode((int)response.StatusCode, problem);
        }

        _logger.LogError("Unexpected response from Document API for Document {DocumentId}. Status: {StatusCode}", id, response.StatusCode);

        return StatusCode((int)response.StatusCode);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _httpClient.DeletAsync($"Document/{id}");

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Document {DocumentId} deleted successfully.", id);

            return NoContent();
        }

        if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            _logger.LogWarning("Downstream API returned problem for Document {DocumentId}: {Title} - {Detail}", id, problem?.Title, problem?.Detail);

            return StatusCode((int)response.StatusCode, problem);
        }

        _logger.LogError("Unexpected response from Document API for Document {DocumentId}. Status: {StatusCode}", id, response.StatusCode);

        return StatusCode((int)response.StatusCode);
    }
}
