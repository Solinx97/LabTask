using LabTask.Domain.Aggregates;
using LabTask.Domain.Data;
using MediatR;

namespace LabTask.Application.Commands.UpdateDocument;

internal class UpdateDocumentHandler(IGenericRepository<Document> repository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateDocumentCommand>
{
    private readonly IGenericRepository<Document> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateDocumentCommand request, CancellationToken ct)
    {
        var document = await _repository.GetByIdAsync(request.Id, ct);

        document.Edit(request.Name, request.Description);

        await _unitOfWork.SaveChangesAsync(ct);
    }
}
