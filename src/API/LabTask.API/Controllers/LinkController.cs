using LabTask.Application.Commands.CreateLink;
using LabTask.Application.Commands.DeleteLink;
using LabTask.Application.Queries.GetLinksByUserId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LabTask.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class LinkController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create(CreateLinkCommand command)
    {
        await _mediator.Send(command);

        return Ok();
    }

    [HttpGet("getByUserId/{id}")]
    public async Task<IActionResult> GetByUserId(Guid id, int page, int pageSize)
    {
        var links = await _mediator.Send(new GetLinksByUserIdQuery(id, page, pageSize));

        return Ok(links);
    }

    [HttpDelete("documents/{documentId}/links/{linkId}")]
    public async Task<IActionResult> Delete(Guid documentId, Guid linkId)
    {
        await _mediator.Send(new DeleteLinkCommand(linkId, documentId));

        return NoContent();
    }
}
