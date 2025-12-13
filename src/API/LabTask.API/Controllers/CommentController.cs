using LabTask.Application.Commands.CreateComment;
using LabTask.Application.Commands.DeleteComment;
using LabTask.Application.Commands.UpdateComment;
using LabTask.Application.DTOs;
using LabTask.Application.Queries.GetCommentsByDocumentId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LabTask.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class CommentController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create(CreateCommentCommand command)
    {
        await _mediator.Send(command);

        return Ok();
    }

    [HttpGet("getByDocumentId/{id}")]
    public async Task<IActionResult> GetByDocumentId(Guid id, int page, int pageSize)
    {
        var comments = await _mediator.Send(new GetCommentsByDocumentIdQuery(id, page, pageSize));

        return Ok(comments);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PartialUpdate(Guid id, CommentDto dto)
    {
        var command = new UpdateCommentCommand(id, dto.Content, dto.DocumentId);

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("documents/{documentId}/comments/{commentId}")]
    public async Task<IActionResult> Delete(Guid documentId, Guid commentId)
    {
        await _mediator.Send(new DeleteCommentCommand(commentId, documentId));

        return NoContent();
    }
}
