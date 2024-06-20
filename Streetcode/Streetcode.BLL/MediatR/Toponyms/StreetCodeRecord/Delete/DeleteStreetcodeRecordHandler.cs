using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Toponyms.StreetCodeRecord.Delete
{
    public class DeleteStreetcodeRecordHandler : IRequestHandler<DeleteStreetcodeRecordQuery, Result<StreetcodeRecordDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public DeleteStreetcodeRecordHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<StreetcodeRecordDTO>> Handle(DeleteStreetcodeRecordQuery request, CancellationToken cancellationToken)
        {
            var record = await _repositoryWrapper.StreetcodeToponymRepository.GetFirstOrDefaultAsync(p => p.StreetcodeId == request.StreetcodeId &&
                                                                                                          p.ToponymId == request.ToponymId);
            if (record == null)
            {
                const string errorMsg = "No partner with such id";
                _logger.LogError(request, errorMsg);
                return Result.Fail(errorMsg);
            }
            else
            {
                try
                {
                    _repositoryWrapper.StreetcodeToponymRepository.Delete(record);
                    _repositoryWrapper.SaveChanges();
                    return Result.Ok(_mapper.Map<StreetcodeRecordDTO>(record));
                }
                catch (Exception ex)
                {
                    _logger.LogError(request, ex.Message);
                    return Result.Fail(ex.Message);
                }
            }
        }
    }
}
