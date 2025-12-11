using LabTask.Domain.Aggregates;
using LabTask.Domain.Data;
using MediatR;

namespace LabTask.Application.Commands.DeleteDocument;

public class DeleteDocumentHandler(IGenericRepository<Document> repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteDocumentCommand>
{
    private readonly IGenericRepository<Document> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteDocumentCommand request, CancellationToken ct)
    {
        await _repository.DeletedAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}
