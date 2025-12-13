using LabTask.Application.DTOs;
using MediatR;

namespace LabTask.Application.Queries.GetHistoryByUserId;

public record GetHistoryByUserIdQuery(
    Guid UserId,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<DocumentDto>>;