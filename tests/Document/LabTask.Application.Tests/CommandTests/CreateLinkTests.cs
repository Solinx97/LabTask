using LabTask.Application.Commands.CreateLink;
using LabTask.Application.DTOs;
using LabTask.Domain.Aggregates;
using LabTask.Domain.Data;
using Moq;

namespace LabTask.Application.Tests.CommandTests;

public class CreateLinkTests
{
    [Fact]
    public async Task Handle_Link_ShouldCreateNewLink()
    {
        // Arrange
        var documentId = Guid.Parse("5211dfe5-d587-4b5f-9aec-7102901b73db");
        var ownerId = Guid.Parse("1311dfe5-d587-4b5f-9aec-7102901b73db");
        var userId = Guid.Parse("5311dfe5-d587-4b5f-9aec-7102901b73db");
        var uri = Guid.Parse("2311dfe5-d587-4b5f-9aec-7102901b73db");
        var expireAt = DateTimeOffset.Parse("1/20/2026 7:17:46 AM +00:00");

        var link = Link.Create(documentId, ownerId, userId, expireAt);

        var linkDto = new LinkDto(link.Id, uri, documentId, ownerId, userId, expireAt);

        var mockRepository = new Mock<IGenericRepository<Link>>();
        var mockUoW= new Mock<IUnitOfWork>();

        mockRepository.Setup(m => m.AddAsync(It.IsAny<Link>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var handler = new CreateLinkHandler(mockRepository.Object, mockUoW.Object);

        // Act
        var result = await handler.Handle(new CreateLinkCommand(documentId, ownerId, userId, expireAt), CancellationToken.None);

        // Assert
        Assert.NotNull(result);

        // Verify
        mockRepository.Verify(m => m.AddAsync(It.IsAny<Link>(), It.IsAny<CancellationToken>()), Times.Once);
        mockUoW.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
