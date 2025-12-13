using LabTask.Application.DTOs;
using MediatR;

namespace LabTask.Application.Queries.GetLinksByOwnerId;

public record GetLinksByOwnerIdQuery(
    Guid OwnerId,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<LinkDto>>;