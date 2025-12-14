using AutoMapper;
using LabTask.Application.DTOs;
using LabTask.Domain.Aggregates;
using LabTask.Domain.Data;
using MediatR;

namespace LabTask.Application.Queries.GetDocument;

public class GetDocumentHandler(IGenericRepository<Document> repository, IMapper mapper) : IRequestHandler<GetDocumentQuery, DocumentDto>
{
    private readonly IGenericRepository<Document> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<DocumentDto> Handle(GetDocumentQuery request, CancellationToken ct)
    {
        var document = await _repository.GetByIdAsync(request.Id, ct);
        var map = _mapper.Map<DocumentDto>(document);

        return map;
    }
}
