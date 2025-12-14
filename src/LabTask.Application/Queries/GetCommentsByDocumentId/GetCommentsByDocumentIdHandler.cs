using AutoMapper;
using LabTask.Application.DTOs;
using LabTask.Domain.Data;
using MediatR;

namespace LabTask.Application.Queries.GetCommentsByDocumentId;

internal class GetCommentsByDocumentIdHandler(ICommentRepository repository, IMapper mapper) : IRequestHandler<GetCommentsByDocumentIdQuery, IEnumerable<CommentDto>>
{
    private readonly ICommentRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CommentDto>> Handle(GetCommentsByDocumentIdQuery request, CancellationToken ct)
    {
        var comments = await _repository.GetByDocumentIdAsync(request.DocumentId, request.Page, request.PageSize, ct);
        var map = _mapper.Map<IEnumerable<CommentDto>>(comments);

        return map;
    }
}
