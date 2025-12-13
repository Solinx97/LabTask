using LabTask.Application.DTOs;
using MediatR;

namespace LabTask.Application.Queries.GetLinksByUserId;

public record GetLinksByUserIdQuery(
    Guid ToUserId,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<LinkDto>>;