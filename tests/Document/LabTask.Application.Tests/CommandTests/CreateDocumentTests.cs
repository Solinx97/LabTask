using LabTask.Application.Commands.CreateDocument;
using LabTask.Application.DTOs;
using LabTask.Domain.Aggregates;
using LabTask.Domain.Data;
using Moq;

namespace LabTask.Application.Tests.CommandTests;

public class CreateDocumentTests
{
    [Fact]
    public async Task Handle_Document_ShouldCreateNewDocument()
    {
        // Arrange
        var userId = Guid.Parse("5311dfe5-d587-4b5f-9aec-7102901b73db");
        var expireAt = DateTimeOffset.Parse("1/20/2026 7:17:46 AM +00:00");

        var name = "test";
        var description = "des";

        var document = Document.Create(name, description, expireAt, userId);

        var documentDto = new DocumentDto(document.Id, name, description, expireAt, userId);

        var mockRepository = new Mock<IGenericRepository<Document>>();
        var mockUoW= new Mock<IUnitOfWork>();

        mockRepository.Setup(m => m.AddAsync(It.IsAny<Document>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var handler = new CreateDocumentHandler(mockRepository.Object, mockUoW.Object);

        // Act
        var result = await handler.Handle(new CreateDocumentCommand(name, description, expireAt, userId), CancellationToken.None);

        // Assert
        Assert.NotNull(result);

        // Verify
        mockRepository.Verify(m => m.AddAsync(It.IsAny<Document>(), It.IsAny<CancellationToken>()), Times.Once);
        mockUoW.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
