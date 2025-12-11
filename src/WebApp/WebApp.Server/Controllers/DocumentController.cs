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
        try
        {
            var responseMessage = await _httpClient.PostAsync("Document", JsonContent.Create(item));
            responseMessage.EnsureSuccessStatusCode();

            return Ok();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogError(ex, "Some issues during create a new document. Please, check your data and try one more time.");

            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Some issues during create a new document. Please, try one more time late.");

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync("Document");
            responseMessage.EnsureSuccessStatusCode();

            var documents = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DocumentModel>>();

            return Ok(documents);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogError(ex, "Some issues during get all documents. Please, check your data and try one more time.");

            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Some issues during get all documents. Please, try one more time late.");

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"Document/{id}");
            responseMessage.EnsureSuccessStatusCode();

            var document = await responseMessage.Content.ReadFromJsonAsync<DocumentModel>();

            return Ok(document);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogError(ex, "Some issues during get document. Please, check your data and try one more time.");

            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Some issues during get document. Please, try one more time late.");

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpGet("getByUserId/{id}")]
    public async Task<IActionResult> GetByUserId(Guid id)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"Document/getByUserId/{id}");
            responseMessage.EnsureSuccessStatusCode();

            var documents = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DocumentModel>>();

            return Ok(documents);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogError(ex, "Some issues during get documents by user ID. Please, check your data and try one more time.");

            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Some issues during get documents by user ID. Please, try one more time late.");

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PartialUpdate(Guid id, [FromBody] DocumentModel item)
    {
        try
        {
            if (id != item.Id)
            {
                return BadRequest("Route ID and body ID do not match.");
            }

            var responseMessage = await _httpClient.PatchAsync($"Document/{id}", JsonContent.Create(item));
            responseMessage.EnsureSuccessStatusCode();

            return NoContent();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogError(ex, "Some issues during updating document. Please, check your data and try one more time.");

            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Some issues during updating document. Please, try one more time late.");

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var responseMessage = await _httpClient.DeletAsync($"Document/{id}");
            responseMessage.EnsureSuccessStatusCode();

            return NoContent();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogError(ex, "Some issues during deleting document. Please, check your data and try one more time.");

            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Some issues during deleting document. Please, try one more time late.");

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }
}
