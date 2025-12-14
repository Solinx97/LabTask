using AutoMapper;
using LabTask.Application.DTOs;
using LabTask.Application.Queries.GetLinksByOwnerId;
using LabTask.Domain.Aggregates;
using LabTask.Domain.Data;
using Moq;

namespace LabTask.Application.Tests.QueryTests;

public class GetLinksByOwnerIdTests
{
    [Fact]
    public async Task Handle_Links_ShouldReturnLinksByOwnerId()
    {
        // Arrange
        var id = Guid.Parse("7311dfe5-d587-4b5f-9aec-7102901b73db");
        var documentId = Guid.Parse("7311dfe5-d587-4b5f-9aec-7102901b73db");
        var userId = Guid.Parse("5311dfe5-d587-4b5f-9aec-7102901b73db");
        var ownerId = Guid.Parse("6311dfe5-d587-4b5f-9aec-7102901b73db");
        var uri = Guid.Parse("9311dfe5-d587-4b5f-9aec-7102901b73db");
        var expiresAt = DateTimeOffset.Parse("8/20/2026 7:17:46 AM +00:00");

        var page = 0;
        var pageSize = 10;

        var links = new List<Link>
        {
            Link.Create(documentId, ownerId, userId, expiresAt)
        };

        var linksDto = new List<LinkDto>
        {
            new(links[0].Id,uri, documentId, ownerId, userId, expiresAt)
        };

        var mockRepository = new Mock<ILinkRepository>();
        var mockMapper = new Mock<IMapper>();

        mockRepository.Setup(m => m.GetByOwnerIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(links);
        mockMapper.Setup(m => m.Map<IEnumerable<LinkDto>>(It.IsAny<IEnumerable<Link>>())).Returns(linksDto);

        var handler = new GetLinksByOwnerIdHandler(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await handler.Handle(new GetLinksByOwnerIdQuery(ownerId, documentId, page, pageSize), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);

        // Verify
        mockRepository.Verify(m => m.GetByOwnerIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        mockMapper.Verify(m => m.Map<IEnumerable<LinkDto>>(It.IsAny<IEnumerable<Link>>()), Times.Once);
    }
}
