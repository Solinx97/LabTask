using LabTask.Domain.Aggregates;
using LabTask.Domain.Data;
using MediatR;

namespace LabTask.Application.Commands.CreateLink;

internal class CreateLinkHandler(IGenericRepository<Link> repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateLinkCommand, Link>
{
    private readonly IGenericRepository<Link> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Link> Handle(CreateLinkCommand request, CancellationToken ct)
    {
        var comment = Link.Create(request.DocumentId, request.OwnerId, request.ToUserId, request.ExpireAt);
        await _repository.AddAsync(comment, ct);

        await _unitOfWork.SaveChangesAsync(ct);

        return comment;
    }
}
