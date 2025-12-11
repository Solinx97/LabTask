using LabTask.Application.Commands.CreateDocument;
using LabTask.Application.Commands.DeleteDocument;
using LabTask.Application.Commands.UpdateDocument;
using LabTask.Application.DTOs;
using LabTask.Application.Queries.GetAllDocuments;
using LabTask.Application.Queries.GetDocument;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LabTask.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class DocumentController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create(CreateDocumentCommand command)
    {
        await _mediator.Send(command);

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var documents = await _mediator.Send(new GetAllDocumentsQuery());

        return Ok(documents);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var document = await _mediator.Send(new GetDocumentQuery(id));

        return Ok(document);
    }

    [HttpGet("getByUserId/{id}")]
    public async Task<IActionResult> GetByUserId(Guid id)
    {
        var documents = await _mediator.Send(new GetDocumentsByUserIdQuery(id));

        return Ok(documents);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PartialUpdate(Guid id, DocumentDto dto)
    {
        var command = new UpdateDocumentCommand(id, dto.Name, dto.Description);

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteDocumentCommand(id));

        return NoContent();
    }
}
