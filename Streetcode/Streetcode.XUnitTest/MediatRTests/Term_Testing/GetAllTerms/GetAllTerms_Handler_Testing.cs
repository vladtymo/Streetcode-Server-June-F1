using AutoMapper;
using FluentResults;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Mapping.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.Term.GetAll;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.MediatRTests.Term_Testing.MapperConfigure;
using Streetcode.XUnitTest.MediatRTests.Term_Testing.Mocker;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Term_Testing.GetAllTerms
{
    public class GetAllTerms_Handler_Testing
    {
        [Fact]
        public async Task GetAllTerms_ShouldReturn_A_Collection_Of_Terms()
        {
            //Assign

            var mockRepo = new MockTermRepo();

            IMapper? map = Mapper_Configurator.Create<TermProfile>();

            GetAllTermsQuery querry = new GetAllTermsQuery();

            Mock<ILoggerService> logerMock = new Mock<ILoggerService>();
            logerMock.Setup(l => l.LogError(querry, "Cannot find any term!"));

            Mock<IRepositoryWrapper> wrapperMock = new Mock<IRepositoryWrapper>();
            wrapperMock.Setup(w => w.TermRepository).Returns(mockRepo);

            GetAllTermsHandler handler = new GetAllTermsHandler(wrapperMock.Object, map, logerMock.Object);

            //Act

            var Result = await handler.Handle(querry, CancellationToken.None);

            //Assert

            Assert.IsType<Result<IEnumerable<TermDTO>>>(Result);

            Assert.True(Result.Value.Count() == MockTermRepo.Terms.Count());
        }
    }
}
