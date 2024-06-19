using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline.Create;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.Update
{
    public class UpdateTimelineItemHandler : IRequestHandler<UpdateTimelineItemCommand, Result<UpdateTimelineItemDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public UpdateTimelineItemHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<UpdateTimelineItemDTO>> Handle(UpdateTimelineItemCommand request, CancellationToken cancellationToken)
        {
            return Result.Ok();
        }
    }
}
