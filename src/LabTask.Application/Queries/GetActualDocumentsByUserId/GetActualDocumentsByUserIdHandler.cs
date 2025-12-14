using AutoMapper;
using LabTask.Application.DTOs;
using LabTask.Domain.Data;
using MediatR;

namespace LabTask.Application.Queries.GetActualDocumentsByUserId;

public class GetActualDocumentsByUserIdHandler(IDocumentRepository repository, IMapper mapper) : IRequestHandler<GetActualDocumentsByUserIdQuery, IEnumerable<DocumentDto>>
{
    private readonly IDocumentRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DocumentDto>> Handle(GetActualDocumentsByUserIdQuery request, CancellationToken ct)
    {
        var documents = await _repository.GetActualDocumentsAsync(request.UserId, request.Page, request.PageSize, ct);
        var map = _mapper.Map<IEnumerable<DocumentDto>>(documents);

        return map;
    }
}
