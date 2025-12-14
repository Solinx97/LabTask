using LabTask.Application.Commands.DeleteLink;
using LabTask.Domain.Aggregates;
using LabTask.Domain.Data;
using Moq;

namespace LabTask.Application.Tests.CommandTests;

public class DeleteLinkTests
{
    [Fact]
    public async Task Handle_ShouldDeleteExistLink()
    {
        // Arrange
        var linkId = Guid.Parse("5211dfe5-d587-4b5f-9aec-7102901b73db");

        var mockRepository = new Mock<IGenericRepository<Link>>();
        var mockUoW= new Mock<IUnitOfWork>();

        mockRepository.Setup(m => m.DeletedAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var handler = new DeleteLinkHandler(mockRepository.Object, mockUoW.Object);

        // Act
        await handler.Handle(new DeleteLinkCommand(linkId), CancellationToken.None);

        // Assert and Verify
        mockRepository.Verify(m => m.DeletedAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        mockUoW.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
