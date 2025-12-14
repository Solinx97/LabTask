using LabTask.Application.Commands.DeleteComment;
using LabTask.Application.DTOs;
using LabTask.Domain.Aggregates;
using LabTask.Domain.Data;
using Moq;

namespace LabTask.Application.Tests.CommandTests;

public class DeleteCommentTests
{
    [Fact]
    public async Task Handle_ShouldDeleteExistComment()
    {
        // Arrange
        var commentId = Guid.Parse("5211dfe5-d587-4b5f-9aec-7102901b73db");
        var userId = Guid.Parse("5311dfe5-d587-4b5f-9aec-7102901b73db");
        var expireAt = DateTimeOffset.Parse("1/20/2026 7:17:46 AM +00:00");

        var name = "test";
        var description = "des";
        var content = "content";

        var document = Document.Create(name, description, expireAt, userId);
        document.AddComment(content, userId);

        var documentDto = new DocumentDto(document.Id, name, description, expireAt, userId);

        var mockRepository = new Mock<IDocumentRepository>();
        var mockUoW= new Mock<IUnitOfWork>();

        mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(document);

        var handler = new DeleteCommentHandler(mockRepository.Object, mockUoW.Object);

        // Act
        await handler.Handle(new DeleteCommentCommand(commentId, document.Id), CancellationToken.None);

        // Assert and Verify
        mockRepository.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        mockUoW.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
