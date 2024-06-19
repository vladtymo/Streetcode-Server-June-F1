using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline.Create;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;


namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.Create
{
    public class CreateTimelineItemHandler : IRequestHandler<CreateTimelineItemCommand, Result<CreateTimelineItemDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public CreateTimelineItemHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<CreateTimelineItemDTO>> Handle(CreateTimelineItemCommand request, CancellationToken cancellationToken)
        {
            DAL.Entities.Timeline.TimelineItem newTimelineItem =
                _mapper.Map<DAL.Entities.Timeline.TimelineItem>(request.newTimeLine);

            if (string.IsNullOrEmpty(newTimelineItem?.Title))
            {
                const string errorMsg = "Timeline item title is null or empty";
                _logger.LogError(request, errorMsg);
                return Result.Fail<CreateTimelineItemDTO>(errorMsg);
            }

            try
            {
                newTimelineItem.HistoricalContextTimelines.Clear();
                await _repositoryWrapper.TimelineRepository.CreateAsync(newTimelineItem);
                await _repositoryWrapper.SaveChangesAsync();

                foreach (var hcDto in request.newTimeLine.HistoricalContexts!)
                {
                    var historicalContext = await _repositoryWrapper.HistoricalContextRepository.GetFirstOrDefaultAsync(
                        hc => string.Equals(hc.Title, hcDto.Title));

                    if (historicalContext == null)
                    {
                        historicalContext = new DAL.Entities.Timeline.HistoricalContext
                        {
                            Title = hcDto.Title
                        };
                        await _repositoryWrapper.HistoricalContextRepository.CreateAsync(historicalContext);
                        await _repositoryWrapper.SaveChangesAsync();
                    }

                    newTimelineItem.HistoricalContextTimelines.Add(new HistoricalContextTimeline
                    {
                        HistoricalContextId = historicalContext.Id,
                        HistoricalContext = historicalContext,
                        TimelineId = newTimelineItem.Id,
                        Timeline = newTimelineItem
                    });
                }

                await _repositoryWrapper.SaveChangesAsync();
                return Result.Ok(_mapper.Map<CreateTimelineItemDTO>(newTimelineItem));
            }
            catch (Exception ex)
            {
                _logger.LogError(request, ex.Message);
                return Result.Fail<CreateTimelineItemDTO>(ex.Message);
            }
        }
    }
}
