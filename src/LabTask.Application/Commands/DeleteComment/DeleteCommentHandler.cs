using LabTask.Domain.Data;
using LabTask.Domain.Exceptions;
using MediatR;

namespace LabTask.Application.Commands.DeleteComment;

internal class DeleteCommentHandler(IDocumentRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteCommentCommand>
{
    private readonly IDocumentRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteCommentCommand request, CancellationToken ct)
    {
        var document = await _repository.GetByIdAsync(request.DocumentId, request.Id, ct) 
            ?? throw new DomainException($"Document {request.DocumentId} not found");

        document?.RemoveComment(request.Id);

        await _unitOfWork.SaveChangesAsync(ct);
    }
}