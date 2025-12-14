using AutoMapper;
using LabTask.Application.DTOs;
using LabTask.Domain.Data;
using MediatR;

namespace LabTask.Application.Queries.GetLinksByOwnerId;

internal class GetLinksByOwnerIdHandler(ILinkRepository repository, IMapper mapper) : IRequestHandler<GetLinksByOwnerIdQuery, IEnumerable<LinkDto>>
{
    private readonly ILinkRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<LinkDto>> Handle(GetLinksByOwnerIdQuery request, CancellationToken ct)
    {
        var links = await _repository.GetByOwnerIdAsync(request.OwnerId, request.DocumentId, request.Page, request.PageSize, ct);
        var map = _mapper.Map<IEnumerable<LinkDto>>(links);

        return map;
    }
}
