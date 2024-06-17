using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.DTO.Timeline.Create;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Timeline.TimelineItem.Create;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.TimelineTests
{
    public class CreateTimelineItemHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;

        public CreateTimelineItemHandlerTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenTitleIsNullOrEmpty()
        {
            // Arrange
            var request = new CreateTimelineItemCommand(new CreateTimelineItemDTO { Title = null });
            var handler = CreateHandler();
            var newTimelineItem = new TimelineItem { Title = null };
            _mockMapper.Setup(m => m.Map<TimelineItem>(It.IsAny<CreateTimelineItemDTO>())).Returns(newTimelineItem);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.First().Message.Should().Be("Timeline item title is null or empty");
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenExceptionIsThrown()
        {
            // Arrange
            var request = GetValidCreateTimelineItemRequest();
            SetupMockForException(request);
            var handler = CreateHandler();
            var command = new CreateTimelineItemCommand(request);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.First().Message.Should().Be("Database error");
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccessResult_WhenOperationIsSuccessful()
        {
            // Arrange
            var request = GetValidCreateTimelineItemRequest();
            SetupMockForSuccess(request);
            var handler = CreateHandler();
            var command = new CreateTimelineItemCommand(request);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(request);
        }

        private CreateTimelineItemHandler CreateHandler()
        {
            return new CreateTimelineItemHandler(
                _mockRepositoryWrapper.Object,
                _mockMapper.Object,
                _mockLogger.Object);
        }

        private void SetupMockForException(CreateTimelineItemDTO request)
        {
            var newTimelineItem = new TimelineItem { Title = request.Title };
            _mockMapper.Setup(m => m.Map<TimelineItem>(It.IsAny<CreateTimelineItemDTO>())).Returns(newTimelineItem);
            _mockRepositoryWrapper.Setup(r => r.TimelineRepository.CreateAsync(It.IsAny<TimelineItem>())).Throws(new Exception("Database error"));
        }

        private void SetupMockForSuccess(CreateTimelineItemDTO request)
        {
            var historicalContexts = new List<HistoricalContext>
            {
                new HistoricalContext { Title = "Context1", Id = 1 },
                new HistoricalContext { Title = "Context2", Id = 2 }
            };
            var newTimelineItem = new TimelineItem 
            { 
                Title = request.Title, 
                Id = 1, 
                Date = DateTime.UtcNow, 
                DateViewPattern = DateViewPattern.MonthYear, 
                StreetcodeId = 2 
            };

            _mockMapper.Setup(m => m.Map<TimelineItem>(It.IsAny<CreateTimelineItemDTO>())).Returns(newTimelineItem);
            _mockRepositoryWrapper.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
            _mockRepositoryWrapper.Setup(r => r.TimelineRepository.CreateAsync(It.IsAny<TimelineItem>())).Returns(Task.FromResult(newTimelineItem));

            _mockRepositoryWrapper.Setup(r => r.HistoricalContextRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<HistoricalContext, bool>>>(), 
                It.IsAny<Func<IQueryable<HistoricalContext>, IIncludableQueryable<HistoricalContext, object>>>())).ReturnsAsync((Expression<Func<HistoricalContext, bool>> predicate, Func<IQueryable<HistoricalContext>, IIncludableQueryable<HistoricalContext, object>> include) =>
                historicalContexts.FirstOrDefault(predicate.Compile()));

            _mockRepositoryWrapper.Setup(r => r.HistoricalContextRepository.CreateAsync(It.IsAny<HistoricalContext>())).Returns(Task.FromResult(new HistoricalContext()));

            _mockMapper.Setup(m => m.Map<CreateTimelineItemDTO>(It.IsAny<TimelineItem>())).Returns(request);
        }



        private CreateTimelineItemDTO GetValidCreateTimelineItemRequest()
        {
            return new CreateTimelineItemDTO
            {
                Title = "Valid Title",
                HistoricalContexts = new List<HistoricalContextDTO>
                {
                    new HistoricalContextDTO { Title = "Context1" },
                    new HistoricalContextDTO { Title = "Context2" }
                }
            };
        }
    }
}
