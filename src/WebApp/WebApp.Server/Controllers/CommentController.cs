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
public class CommentController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<CommentController> _logger;

    public CommentController(IOptions<APIServer> server, IHttpClientHelper httpClient, ILogger<CommentController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        _httpClient.APIUrl = server.Value.Document;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommentModel item)
    {
        var response = await _httpClient.PostAsync("Comment", JsonContent.Create(item));

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Comment {CommentId} created successfully for UserId: {UserId}", item.Id, item.UserId);
            return Ok();
        }

        if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            _logger.LogWarning("Downstream API returned problem for Comment {CommentId}: {Title} - {Detail}", item.Id, problem?.Title, problem?.Detail);

            return StatusCode((int)response.StatusCode, problem);
        }

        _logger.LogError("Unexpected response from Comment API for Comment {CommentId}. Status: {StatusCode}", item.Id, response.StatusCode);

        return StatusCode((int)response.StatusCode);

    }

    [HttpGet("getByDocumentId/{id}")]
    public async Task<IActionResult> GetByDocumentId(Guid id, [FromQuery] int page, [FromQuery] int pageSize)
    {
        var response = await _httpClient.GetAsync($"Comment/getByDocumentId/{id}?page={page}&pageSize={pageSize}");

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Comments by Document {DcoumentId} extracted successfully.", id);

            var documents = await response.Content.ReadFromJsonAsync<IEnumerable<CommentModel>>();

            return Ok(documents);
        }

        if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            _logger.LogWarning("Downstream API returned problem for Document {DcoumentId}: {Title} - {Detail}", id, problem?.Title, problem?.Detail);

            return StatusCode((int)response.StatusCode, problem);
        }

        _logger.LogError("Unexpected response from Comment API for Document {CommentId}. Status: {StatusCode}", id, response.StatusCode);

        return StatusCode((int)response.StatusCode);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PartialUpdate(Guid id, [FromBody] CommentModel item)
    {
        if (id != item.Id)
        {
            _logger.LogWarning("Route ID and body ID do not match.");

            return BadRequest("Route ID and body ID do not match.");
        }

        var response = await _httpClient.PatchAsync($"Comment/{id}", JsonContent.Create(item));

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Comment {CommentId} update successfully.", id);

            return NoContent();
        }

        if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            _logger.LogWarning("Downstream API returned problem for Comment {CommentId}: {Title} - {Detail}", id, problem?.Title, problem?.Detail);

            return StatusCode((int)response.StatusCode, problem);
        }

        _logger.LogError("Unexpected response from Comment API for Comment {CommentId}. Status: {StatusCode}", id, response.StatusCode);

        return StatusCode((int)response.StatusCode);
    }

    [HttpDelete("documents/{documentId}/comments/{commentId}")]
    public async Task<IActionResult> Delete(Guid documentId, Guid commentId)
    {
        var response = await _httpClient.DeletAsync($"Comment/documents/{documentId}/comments/{commentId}");

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Comment {CommentId} deleted successfully.", commentId);

            return NoContent();
        }

        if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            _logger.LogWarning("Downstream API returned problem for Comment {CommentId}: {Title} - {Detail}", commentId, problem?.Title, problem?.Detail);

            return StatusCode((int)response.StatusCode, problem);
        }

        _logger.LogError("Unexpected response from Comment API for Comment {CommentId}. Status: {StatusCode}", commentId, response.StatusCode);

        return StatusCode((int)response.StatusCode);
    }
}
