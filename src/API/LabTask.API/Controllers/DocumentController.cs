using LabTask.Application.Commands.CreateDocument;
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var document = await _mediator.Send(new GetDocumentQuery(id));

        return Ok(document);
    }
}
