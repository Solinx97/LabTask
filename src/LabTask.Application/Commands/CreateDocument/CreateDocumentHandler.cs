using LabTask.Domain.Aggregates;
using LabTask.Domain.Data;
using MediatR;

namespace LabTask.Application.Commands.CreateDocument;

internal class CreateDocumentHandler(IGenericRepository<Document> repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateDocumentCommand, Document>
{
    private readonly IGenericRepository<Document> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Document> Handle(CreateDocumentCommand request, CancellationToken ct)
    {
        var document = Document.Create(request.Name, request.Description, request.ExpireAt, request.UserId);
        await _repository.AddAsync(document);

        await _unitOfWork.SaveChangesAsync(ct);

        return document;
    }
}
