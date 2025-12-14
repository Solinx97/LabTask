using LabTask.Application.Commands.CreateDocument;
using LabTask.Application.Commands.DeleteDocument;
using LabTask.Application.Commands.UpdateDocument;
using LabTask.Application.DTOs;
using LabTask.Application.Queries.DocumentStatisticsByRange;
using LabTask.Application.Queries.DocumentStatisticsByYear;
using LabTask.Application.Queries.GetActualDocumentsByUserId;
using LabTask.Application.Queries.GetDocument;
using LabTask.Application.Queries.GetDocumentsByName;
using LabTask.Application.Queries.GetExpiredDocumentsByUserId;
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
        var document = await _mediator.Send(command);

        return Ok(document);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var document = await _mediator.Send(new GetDocumentQuery(id));

        return Ok(document);
    }

    [HttpGet("getByName/{id}")]
    public async Task<IActionResult> GetByName(Guid id, string name)
    {
        var documents = await _mediator.Send(new GetDocumentsByNameQuery(id, name));

        return Ok(documents);
    }

    [HttpGet("getActualByUserId/{id}")]
    public async Task<IActionResult> GetActualByUserId(Guid id, int page, int pageSize)
    {
        var documents = await _mediator.Send(new GetActualDocumentsByUserIdQuery(id, page, pageSize));

        return Ok(documents);
    }

    [HttpGet("getExpiredDocumentsByUserId/{id}")]
    public async Task<IActionResult> GetHistoryByUserUd(Guid id, int page, int pageSize)
    {
        var documents = await _mediator.Send(new GetExpiredDocumentsByUserIdQuery(id, page, pageSize));

        return Ok(documents);
    }

    [HttpGet("statisticsByYear/{id}")]
    public async Task<IActionResult> StatisticsByYear(Guid id)
    {
        var statistics = await _mediator.Send(new DocumentStatisticsByYearQuery(id));

        return Ok(statistics);
    }

    [HttpGet("statisticsByRange/{id}")]
    public async Task<IActionResult> StatisticsByRange(Guid id, DateTime startedAt, DateTime finishedAt)
    {
        var statistics = await _mediator.Send(new DocumentStatisticsByRangeQuery(id, startedAt, finishedAt));

        return Ok(statistics);
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
