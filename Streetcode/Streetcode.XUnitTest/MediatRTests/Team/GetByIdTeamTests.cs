namespace Streetcode.XUnitTest.MediatRTests.Team
{
    using System.Linq.Expressions;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore.Query;
    using Moq;
    using Streetcode.BLL.DTO.Team;
    using Streetcode.BLL.Interfaces.Logging;
    using Streetcode.BLL.MediatR.Team.GetById;
    using Streetcode.DAL.Entities.Streetcode.TextContent;
    using Streetcode.DAL.Entities.Team;
    using Streetcode.DAL.Repositories.Interfaces.Base;
    using Xunit;

    /// <summary>
    /// Testing Team GetAll.
    /// </summary>
    public class GetByIdTeamTests
    {
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepositoryWrapper> mockRepositoryWrapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly GetByIdTeamHandler handler;

        private const string ERROR_MSG = "Cannot find any team with corresponding id: ";

        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdTeamTests"/> class.
        /// </summary>
        public GetByIdTeamTests()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.handler = new GetByIdTeamHandler(
                mockRepositoryWrapper.Object, mockMapper.Object, mockLogger.Object);
        }

        /// <summary>
        /// Checks Handle to success if team contains team_member with correct id.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenTeamIsNotNull()
        {
            // Arrange
            int id = 1;
            var memberResult = membersDTO.SingleOrDefault(m => m.Id == id);
            ArrangeMockWrapper(id);
            mockMapper
            .Setup(m => m.Map<TeamMemberDTO>(members[0]))
            .Returns(memberResult);
            var query = new GetByIdTeamQuery(id);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        /// <summary>
        /// Checks Handle to return correct team_member with correct id.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_SHould_ReturnTeam_WhenCorrectId()
        {
            // Arrange
            int id = 1;
            var memberResult = membersDTO.SingleOrDefault(m => m.Id == id);
            ArrangeMockWrapper(id);

            mockMapper
            .Setup(m => m.Map<TeamMemberDTO>(members[0]))
            .Returns(memberResult);
            var query = new GetByIdTeamQuery(id);

            // Act  
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(memberResult, result.Value);
        }

        /// <summary>
        /// Checks Handle to fail if id don`t exist.
        /// </summary> 
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>

        [Fact]
        public async Task Handle_Should_ReturnFailResult_WhenNonExistingId()
        {
            // Arrange
            int id = -1;
            ArrangeMockWrapper();
            var query = new GetByIdTeamQuery(id);
            var memberResult = membersDTO.SingleOrDefault(m => m.Id == id);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
        }

        /// <summary>
        /// Checks Handle to error if id don`t exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_Should_ReturnErrors_WhenNonExistingId()
        {
            // Arrange
            int id = -1;
            ArrangeMockWrapper();
            var query = new GetByIdTeamQuery(id);
            var memberResult = membersDTO.SingleOrDefault(m => m.Id == id);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Errors);
        }

        /// <summary>
        /// Checks Handle to error "Cannot find any team with corresponding id:" if id don`t exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Handle_Should_SpecificError_WhenNonExistingId()
        {
            // Arrange
            int id = -1;
            ArrangeMockWrapper();
            var query = new GetByIdTeamQuery(id);
            var memberResult = membersDTO.SingleOrDefault(m => m.Id == id);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            mockLogger.Verify(l => l.LogError(query, $"{ERROR_MSG}{id}"), Times.Once);
            Assert.Contains(result.Reasons, m => m.Message == $"{ERROR_MSG}{id}");
        }

        private readonly List<TeamMemberDTO> membersDTO =
            new()
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
            new()
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

        private void ArrangeMockWrapper(int? id = null)
        {
            if (id != null)
            {
                mockRepositoryWrapper
            .Setup(r => r.TeamRepository.GetSingleOrDefaultAsync(
                It.IsAny<Expression<Func<TeamMember, bool>>>(),
                It.IsAny<Func<IQueryable<TeamMember>, IIncludableQueryable<TeamMember, object>>>()))
            .ReturnsAsync(members[(int)id - 1]);
            }
            else
            {
                mockRepositoryWrapper
            .Setup(r => r.TeamRepository.GetSingleOrDefaultAsync(
                It.IsAny<Expression<Func<TeamMember, bool>>>(),
                It.IsAny<Func<IQueryable<TeamMember>, IIncludableQueryable<TeamMember, object>>>()))
            .ReturnsAsync((TeamMember)null);
            }
        }
    }
}
