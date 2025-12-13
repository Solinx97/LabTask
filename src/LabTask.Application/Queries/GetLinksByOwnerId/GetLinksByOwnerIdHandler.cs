using LabTask.Application.DTOs;
using LabTask.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LabTask.Application.Queries.GetLinksByOwnerId;

internal class GetLinksByOwnerIdHandler(AppDbContext db) : IRequestHandler<GetLinksByOwnerIdQuery, IEnumerable<LinkDto>>
{
    private readonly AppDbContext _db = db;

    public async Task<IEnumerable<LinkDto>> Handle(GetLinksByOwnerIdQuery request, CancellationToken ct)
    {
        var comments = await _db.Link
            .Where(d => d.OwnerId == request.OwnerId)
            .Select(c => new LinkDto(c.Id, c.Uri, c.DocumentId, c.OwnerId, c.ToUserId, c.ExpireAt))
            .Skip(request.Page * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return comments;
    }
}
