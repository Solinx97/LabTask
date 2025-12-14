using LabTask.Application.Queries.DocumentStatisticsByRange;
using LabTask.Domain.Data;
using LabTask.Domain.Helpers;
using Moq;

namespace LabTask.Application.Tests.QueryTests;

public class DocumentStatisticsByRangeTests
{
    [Fact]
    public async Task Handle_Statistics_ShouldReturnStatisticsForDocumentsByRange()
    {
        // Arrange
        var userId = Guid.Parse("5311dfe5-d587-4b5f-9aec-7102901b73db");
        var startedAt = DateTimeOffset.Parse("1/20/2026 7:17:46 AM +00:00");
        var finishedAt = DateTimeOffset.Parse("11/20/2026 7:17:46 AM +00:00");

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

        mockRepository.Setup(m => m.GetStatisticsAsync(It.IsAny<Guid>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>())).ReturnsAsync(statistics);

        var handler = new DocumentStatisticsByRangeHandler(mockRepository.Object);

        // Act
        var result = await handler.Handle(new DocumentStatisticsByRangeQuery(userId, startedAt, finishedAt), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);

        // Verify
        mockRepository.Verify(m => m.GetStatisticsAsync(It.IsAny<Guid>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
