using LabTask.Application.Commands.CreateComment;
using LabTask.Application.DTOs;
using LabTask.Domain.Data;
using LabTask.Domain.Entities;
using Moq;

namespace LabTask.Application.Tests.CommandTests;

public class CreateCommentTests
{
    [Fact]
    public async Task Handle_Comment_ShouldCreateNewComment()
    {
        // Arrange
        var documentId = Guid.Parse("5211dfe5-d587-4b5f-9aec-7102901b73db");
        var userId = Guid.Parse("5311dfe5-d587-4b5f-9aec-7102901b73db");

        var content = "test";

        var comment = Comment.Create(content, documentId, userId);

        var commentDto = new CommentDto(comment.Id, content, documentId, userId);

        var mockRepository = new Mock<IGenericRepository<Comment>>();
        var mockUoW= new Mock<IUnitOfWork>();

        mockRepository.Setup(m => m.AddAsync(It.IsAny<Comment>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var handler = new CreateCommentHandler(mockRepository.Object, mockUoW.Object);

        // Act
        var result = await handler.Handle(new CreateCommentCommand(content, documentId, userId), CancellationToken.None);

        // Assert
        Assert.NotNull(result);

        // Verify
        mockRepository.Verify(m => m.AddAsync(It.IsAny<Comment>(), It.IsAny<CancellationToken>()), Times.Once);
        mockUoW.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
