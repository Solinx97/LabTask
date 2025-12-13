using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
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
        try
        {
            var responseMessage = await _httpClient.PostAsync("Comment", JsonContent.Create(item));
            responseMessage.EnsureSuccessStatusCode();

            return Ok();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogError(ex, "Some issues during create a new comment. Please, check your data and try one more time.");

            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Some issues during create a new comment. Please, try one more time late.");

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync("Comment");
            responseMessage.EnsureSuccessStatusCode();

            var comments = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommentModel>>();

            return Ok(comments);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogError(ex, "Some issues during get all comments. Please, check your data and try one more time.");

            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Some issues during get all comments. Please, try one more time late.");

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpGet("getByDocumentId/{id}")]
    public async Task<IActionResult> GetByDocumentId(Guid id, [FromQuery] int page, [FromQuery] int pageSize)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"Comment/getByDocumentId/{id}?page={page}&pageSize={pageSize}");
            responseMessage.EnsureSuccessStatusCode();

            var comments = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommentModel>>();

            return Ok(comments);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogError(ex, "Some issues during get comments by user ID. Please, check your data and try one more time.");

            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Some issues during get comments by user ID. Please, try one more time late.");

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PartialUpdate(Guid id, [FromBody] CommentModel item)
    {
        try
        {
            if (id != item.Id)
            {
                return BadRequest("Route ID and body ID do not match.");
            }

            var responseMessage = await _httpClient.PatchAsync($"Comment/{id}", JsonContent.Create(item));
            responseMessage.EnsureSuccessStatusCode();

            return NoContent();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogError(ex, "Some issues during updating comment. Please, check your data and try one more time.");

            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Some issues during updating comment. Please, try one more time late.");

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpDelete("documents/{documentId}/comments/{commentId}")]
    public async Task<IActionResult> Delete(Guid documentId, Guid commentId)
    {
        try
        {
            var responseMessage = await _httpClient.DeletAsync($"Comment/documents/{documentId}/comments/{commentId}");
            responseMessage.EnsureSuccessStatusCode();

            return NoContent();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogError(ex, "Some issues during deleting comment. Please, check your data and try one more time.");

            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Some issues during deleting comment. Please, try one more time late.");

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }
}
