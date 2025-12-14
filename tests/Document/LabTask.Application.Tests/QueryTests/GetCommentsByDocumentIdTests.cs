using AutoMapper;
using LabTask.Application.DTOs;
using LabTask.Application.Queries.GetCommentsByDocumentId;
using LabTask.Domain.Data;
using LabTask.Domain.Entities;
using Moq;

namespace LabTask.Application.Tests.QueryTests;

public class GetCommentsByDocumentIdTests
{
    [Fact]
    public async Task Handle_Comments_ShouldReturnCommentsByDocumentId()
    {
        // Arrange
        var documentId = Guid.Parse("7311dfe5-d587-4b5f-9aec-7102901b73db");
        var userId = Guid.Parse("5311dfe5-d587-4b5f-9aec-7102901b73db");
        var expiresAt = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");

        var page = 0;
        var pageSize = 10;

        var comments = new List<Comment>
        {
            Comment.Create("test name", documentId, userId)
        };

        var commentsDto = new List<CommentDto>
        {
            new(comments[0].Id, "test name", documentId, userId)
        };

        var mockRepository = new Mock<ICommentRepository>();
        var mockMapper = new Mock<IMapper>();

        mockRepository.Setup(m => m.GetByDocumentIdAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(comments);
        mockMapper.Setup(m => m.Map<IEnumerable<CommentDto>>(It.IsAny<IEnumerable<Comment>>())).Returns(commentsDto);

        var handler = new GetCommentsByDocumentIdHandler(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await handler.Handle(new GetCommentsByDocumentIdQuery(documentId, page, pageSize), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);

        // Verify
        mockRepository.Verify(m => m.GetByDocumentIdAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        mockMapper.Verify(m => m.Map<IEnumerable<CommentDto>>(It.IsAny<IEnumerable<Comment>>()), Times.Once);
    }
}
