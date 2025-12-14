using AutoMapper;
using LabTask.Application.DTOs;
using LabTask.Application.Queries.GetActualDocumentsByUserId;
using LabTask.Domain.Aggregates;
using LabTask.Domain.Data;
using Moq;

namespace LabTask.Application.Tests.QueryTests;

public class GetActualDocumentsByUserIdTests
{
    [Fact]
    public async Task Handle_Documents_ShouldReturnActualDocuments()
    {
        // Arrange
        var userId = Guid.Parse("5311dfe5-d587-4b5f-9aec-7102901b73db");
        var expiresAt = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");

        var page = 0;
        var pageSize = 10;

        var documents = new List<Document>
        {
            Document.Create("test name", "test description", expiresAt, userId)
        };

        var documentsDto = new List<DocumentDto>
        {
            new(documents[0].Id, "test name", "test description", expiresAt, userId)
        };

        var mockRepository = new Mock<IDocumentRepository>();
        var mockMapper = new Mock<IMapper>();

        mockRepository.Setup(m => m.GetActualDocumentsAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(documents);
        mockMapper.Setup(m => m.Map<IEnumerable<DocumentDto>>(It.IsAny<IEnumerable<Document>>())).Returns(documentsDto);

        var handler = new GetActualDocumentsByUserIdHandler(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await handler.Handle(new GetActualDocumentsByUserIdQuery(userId, page, pageSize), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);

        // Verify
        mockRepository.Verify(m => m.GetActualDocumentsAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        mockMapper.Verify(m => m.Map<IEnumerable<DocumentDto>>(It.IsAny<IEnumerable<Document>>()), Times.Once);
    }
}
