using LabTask.Application.DTOs;
using MediatR;

namespace LabTask.Application.Queries.GetLinksByOwnerId;

public record GetLinksByOwnerIdQuery(
    Guid OwnerId,
    Guid DocumentId,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<LinkDto>>;