using LabTask.Application.Queries.DocumentStatisticsByYear;
using LabTask.Domain.Data;
using LabTask.Domain.Helpers;
using Moq;

namespace LabTask.Application.Tests.QueryTests;

public class DocumentStatisticsByYearTests
{
    [Fact]
    public async Task Handle_Statistics_ShouldReturnStatisticsForDocuments()
    {
        // Arrange
        var userId = Guid.Parse("5311dfe5-d587-4b5f-9aec-7102901b73db");

        var statistics = new List<Statistic>
        {
            new() {
                Year = 2025,
                CreatedAtCount = 1,
                UpdatedAtCount = 2,
                ExpiredAtCount = 3,
            }
        };

        var mockRepository = new Mock<IDocumentRepository>();

        mockRepository.Setup(m => m.GetStatisticsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(statistics);

        var handler = new DocumentStatisticsByYearHandler(mockRepository.Object);

        // Act
        var result = await handler.Handle(new DocumentStatisticsByYearQuery(userId), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);

        // Verify
        mockRepository.Verify(m => m.GetStatisticsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
