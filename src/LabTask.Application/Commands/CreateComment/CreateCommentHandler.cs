using LabTask.Domain.Data;
using LabTask.Domain.Entities;
using MediatR;

namespace LabTask.Application.Commands.CreateComment;

internal class CreateCommentHandler(IGenericRepository<Comment> repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateCommentCommand, Comment>
{
    private readonly IGenericRepository<Comment> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Comment> Handle(CreateCommentCommand request, CancellationToken ct)
    {
        var comment = Comment.Create(request.Content, request.DocumentId, request.UserId);
        await _repository.AddAsync(comment);

        await _unitOfWork.SaveChangesAsync(ct);

        return comment;
    }
}
