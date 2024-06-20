using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources;
using Streetcode.DAL.Repositories.Interfaces.Base;


namespace Streetcode.BLL.MediatR.Toponyms.StreetCodeRecord.Create
{
    public class CreateStreetcodeRecordHandler : IRequestHandler<CreateStreetcodeRecordCommand, Result<StreetcodeRecordDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerService _logger;

        public CreateStreetcodeRecordHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger)
        {
            _mapper = mapper;
            _repository = repositoryWrapper;
            _logger = logger;
        }

        public async Task<Result<StreetcodeRecordDTO>> Handle(CreateStreetcodeRecordCommand request, CancellationToken cancellationToken)
        {
            var newRecord = _mapper.Map<DAL.Entities.Toponyms.StreetcodeToponym>(request.newRecord);

            if(newRecord is null)
            {
                var errorMsgNull = MessageResourceContext.GetMessage(ErrorMessages.FailToConvertNull, request);
                _logger.LogError(request, errorMsgNull);
                return Result.Fail(new Error(errorMsgNull));
            }

           /* var existingIndex = await _repository.StreetcodeToponymRepository.GetFirstOrDefaultAsync(
                predicate: x => newRecord.Streetcode.Index == x.Toponym.Streetcodes.Index);*/


            var createdRecord = await _repository.StreetcodeToponymRepository.CreateAsync(newRecord);

            var isSuccessResult = await _repository.SaveChangesAsync() > 0;

            if(!isSuccessResult)
            {
                var errorMsg = MessageResourceContext.GetMessage(ErrorMessages.FailToCreateA, request);
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<StreetcodeRecordDTO>(createdRecord));
        }
    }
}
