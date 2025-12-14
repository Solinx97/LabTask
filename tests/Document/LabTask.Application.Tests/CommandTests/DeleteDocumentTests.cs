using LabTask.Application.Commands.DeleteDocument;
using LabTask.Domain.Data;
using Moq;

namespace LabTask.Application.Tests.CommandTests;

public class DeleteDocumentTests
{
    [Fact]
    public async Task Handle_ShouldDeleteExistDocument()
    {
        // Arrange
        var documentId = Guid.Parse("5211dfe5-d587-4b5f-9aec-7102901b73db");

        var mockRepository = new Mock<IDocumentRepository>();
        var mockUoW= new Mock<IUnitOfWork>();

        mockRepository.Setup(m => m.DeletedAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var handler = new DeleteDocumentHandler(mockRepository.Object, mockUoW.Object);

        // Act
        await handler.Handle(new DeleteDocumentCommand(documentId), CancellationToken.None);

        // Assert and Verify
        mockRepository.Verify(m => m.DeletedAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        mockUoW.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
