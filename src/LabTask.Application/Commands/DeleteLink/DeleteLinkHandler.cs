using LabTask.Domain.Aggregates;
using LabTask.Domain.Data;
using MediatR;

namespace LabTask.Application.Commands.DeleteLink;

internal class DeleteLinkHandler(IGenericRepository<Link> repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteLinkCommand>
{
    private readonly IGenericRepository<Link> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteLinkCommand request, CancellationToken ct)
    {
        await _repository.DeletedAsync(request.Id, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}
