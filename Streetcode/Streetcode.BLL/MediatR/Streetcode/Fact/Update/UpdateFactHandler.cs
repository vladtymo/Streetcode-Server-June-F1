using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Entities.Feedback;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Realizations.AdditionalContent;

namespace Streetcode.BLL.MediatR.Streetcode.Facts.Update
{
    public class UpdateFactHandler : IRequestHandler<UpdateFactCommand, Result<FactUpdateCreateDto>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        public UpdateFactHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper,  ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<FactUpdateCreateDto>> Handle(UpdateFactCommand request, CancellationToken cancellationToken)
        {
            var existingFact = await _repositoryWrapper.FactRepository.GetFirstOrDefaultAsync(f => f.Id == request.Fact.Id);

            if (existingFact == null)
            {
                const string errorMsg = $"Fact with ID  does not exist";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

      //     var fact = _mapper.Map(request.Fact, existingFact);
              var fact = _mapper.Map<DAL.Entities.Streetcode.TextContent.Fact>(request.Fact);
            if (fact is null)
            {
                const string errorMsg = $"Cannot convert null to fact";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            _repositoryWrapper.FactRepository.Update(fact);
            var resItIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
            if (resItIsSuccess)
            {
                return Result.Ok(_mapper.Map<FactUpdateCreateDto>(fact));
            }
            else
            {
                const string errorMsg = $"Failed to update fact";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
