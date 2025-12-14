using AutoMapper;
using LabTask.Application.DTOs;
using LabTask.Domain.Data;
using MediatR;

namespace LabTask.Application.Queries.GetExpiredDocumentsByUserId;

internal class GetExpiredDocumentsByUserIdHandler(IDocumentRepository repository, IMapper mapper) : IRequestHandler<GetExpiredDocumentsByUserIdQuery, IEnumerable<DocumentDto>>
{
    private readonly IDocumentRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DocumentDto>> Handle(GetExpiredDocumentsByUserIdQuery request, CancellationToken ct)
    {
        var documents = await _repository.GetExpiredDocumentsAsync(request.UserId, request.Page, request.PageSize, ct);
        var map = _mapper.Map<IEnumerable<DocumentDto>>(documents);

        return map;
    }
}
