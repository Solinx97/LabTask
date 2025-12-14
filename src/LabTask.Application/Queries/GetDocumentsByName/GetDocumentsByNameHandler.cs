using AutoMapper;
using LabTask.Application.DTOs;
using LabTask.Domain.Data;
using MediatR;

namespace LabTask.Application.Queries.GetDocumentsByName;

public class GetDocumentsByNameHandler(IDocumentRepository repository, IMapper mapper) : IRequestHandler<GetDocumentsByNameQuery, IEnumerable<DocumentDto>>
{
    private readonly IDocumentRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DocumentDto>> Handle(GetDocumentsByNameQuery request, CancellationToken ct)
    {
        var documents = await _repository.GetDocumentByNameAsync(request.UserId, request.Name, ct);
        var map = _mapper.Map<IEnumerable<DocumentDto>>(documents);

        return map;
    }
}
