using AutoMapper;
using LabTask.Application.DTOs;
using LabTask.Domain.Data;
using MediatR;

namespace LabTask.Application.Queries.GetLinksByUserId;

internal class GetLinksByUserIdHandler(ILinkRepository repository, IMapper mapper) : IRequestHandler<GetLinksByUserIdQuery, IEnumerable<LinkDto>>
{
    private readonly ILinkRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<LinkDto>> Handle(GetLinksByUserIdQuery request, CancellationToken ct)
    {
        var links = await _repository.GetByUserIdAsync(request.ToUserId, request.Page, request.PageSize, ct);
        var map = _mapper.Map<IEnumerable<LinkDto>>(links);

        return map;
    }
}
