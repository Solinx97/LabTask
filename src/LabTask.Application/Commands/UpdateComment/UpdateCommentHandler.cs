using LabTask.Domain.Data;
using LabTask.Domain.Exceptions;
using MediatR;

namespace LabTask.Application.Commands.UpdateComment;

internal class UpdateCommentHandler(IDocumentRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCommentCommand>
{
    private readonly IDocumentRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateCommentCommand request, CancellationToken ct)
    {
        var document = await _repository.GetByIdAsync(request.DocumentId, request.Id, ct) 
            ?? throw new DomainException($"Document {request.DocumentId} not found");

        document.EditComment(request.Content, request.Id);

        await _unitOfWork.SaveChangesAsync(ct);
    }
}
