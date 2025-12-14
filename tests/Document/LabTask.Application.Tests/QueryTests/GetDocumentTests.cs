using AutoMapper;
using LabTask.Application.DTOs;
using LabTask.Application.Queries.GetDocument;
using LabTask.Domain.Aggregates;
using LabTask.Domain.Data;
using Moq;

namespace LabTask.Application.Tests.QueryTests;

public class GetDocumentTests
{
    [Fact]
    public async Task Handle_Document_ShouldReturnDocumentById()
    {
        // Arrange
        var id = Guid.Parse("7311dfe5-d587-4b5f-9aec-7102901b73db");
        var userId = Guid.Parse("5311dfe5-d587-4b5f-9aec-7102901b73db");
        var expiresAt = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");

        var document = Document.Create("test name", "test description", expiresAt, userId);
        var documentDto = new DocumentDto(document.Id, "test name", "test description", expiresAt, userId);

        var mockRepository = new Mock<IGenericRepository<Document>>();
        var mockMapper = new Mock<IMapper>();

        mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(document);
        mockMapper.Setup(m => m.Map<DocumentDto>(It.IsAny<Document>())).Returns(documentDto);

        var handler = new GetDocumentHandler(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await handler.Handle(new GetDocumentQuery(id), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(document.Id, result.Id);

        // Verify
        mockRepository.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        mockMapper.Verify(m => m.Map<DocumentDto>(It.IsAny<Document>()), Times.Once);
    }
}
