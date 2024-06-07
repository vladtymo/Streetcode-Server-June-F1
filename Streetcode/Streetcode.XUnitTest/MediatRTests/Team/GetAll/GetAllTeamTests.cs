namespace Streetcode.XUnitTest.MediatRTests.Team.GetAll
{
    using System.Linq.Expressions;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore.Query;
    using Moq;
    using Streetcode.BLL.DTO.Team;
    using Streetcode.BLL.Interfaces.Logging;
    using Streetcode.BLL.MediatR.Team.GetAll;
    using Streetcode.DAL.Entities.Team;
    using Streetcode.DAL.Repositories.Interfaces.Base;
    using Xunit;

    /// <summary>
    /// Testing Team GetAll.
    /// </summary>
    public class GetAllTeamTests
    {
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepositoryWrapper> mockRepositoryWrapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly GetAllTeamHandler handler;

        private readonly List<TeamMemberDTO> membersDTO =
            new ()
            {
                new TeamMemberDTO
                {
                    Id = 1, FirstName = "Test", LastName = "Test_Last",
                    Description = "Test_desc", IsMain = true,
                    ImageId = 1,
                },
                new TeamMemberDTO
                {
                    Id = 2, FirstName = "Test", LastName = "Test_Last",
                    Description = "Test_desc", IsMain = false,
                    ImageId = 2,
                },
            };

        private readonly List<TeamMember> members =
            new ()
            {
                new TeamMember 
                {
                    Id = 1, FirstName = "Test", LastName = "Test_last",
                    Description = "Test_desc", IsMain = true,
                    ImageId = 1,
                },
                new TeamMember
                {
                    Id = 2, FirstName = "Test", LastName = "Test_last",
                    Description = "Test_desc", IsMain = false,
                    ImageId = 2,
                },
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllTeamTests"/> class.
        /// </summary>
        public GetAllTeamTests()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.handler = new GetAllTeamHandler(mockRepositoryWrapper.Object, mockMapper.Object, mockLogger.Object);
        }

        /// <summary>
        /// Checks Handle to success and return all teams.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task HandleReturnAllTeams()
        {
            // Arrange
            ArrangeMockWrapper(members);
            mockMapper
            .Setup(m => m.Map<IEnumerable<TeamMemberDTO>>(members))
            .Returns(membersDTO);
            var query = new GetAllTeamQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(membersDTO, result.Value);
        }

        /// <summary>
        /// Checks Handle to fail and return error message if no teams.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>

        [Fact]
        public async Task Handle_ShouldReturnFailResult_WhenTeamIsNull()
        {
            // Arrange
            ArrangeMockWrapper(null);
            var query = new GetAllTeamQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.NotNull(result.Errors); 
            Assert.Contains(result.Reasons, m => m.Message == "Cannot find any team");
            mockLogger.Verify(l => l.LogError(query, $"Cannot find any team"), Times.Once);
        }

        private void ArrangeMockWrapper(List<TeamMember>? memb)
        {
            mockRepositoryWrapper
            .Setup(r => r.TeamRepository.GetAllAsync(
                It.IsAny<Expression<Func<TeamMember, bool>>>(),
                It.IsAny<Func<IQueryable<TeamMember>, IIncludableQueryable<TeamMember, object>>>()))
            .ReturnsAsync(memb);
        }
    }
}
