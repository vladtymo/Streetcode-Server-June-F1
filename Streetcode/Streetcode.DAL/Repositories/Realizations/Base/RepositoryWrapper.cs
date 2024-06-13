using System.Transactions;
using Repositories.Interfaces;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Analytics;
using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Entities.Transactions;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Analytics;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Interfaces.Media.Images;
using Streetcode.DAL.Repositories.Interfaces.Newss;
using Streetcode.DAL.Repositories.Interfaces.Partners;
using Streetcode.DAL.Repositories.Interfaces.Source;
using Streetcode.DAL.Repositories.Interfaces.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Team;
using Streetcode.DAL.Repositories.Interfaces.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Toponyms;
using Streetcode.DAL.Repositories.Interfaces.Transactions;
using Streetcode.DAL.Repositories.Interfaces.Users;
using Streetcode.DAL.Repositories.Realizations.AdditionalContent;
using Streetcode.DAL.Repositories.Realizations.Analytics;
using Streetcode.DAL.Repositories.Realizations.Media;
using Streetcode.DAL.Repositories.Realizations.Media.Images;
using Streetcode.DAL.Repositories.Realizations.Newss;
using Streetcode.DAL.Repositories.Realizations.Partners;
using Streetcode.DAL.Repositories.Realizations.Source;
using Streetcode.DAL.Repositories.Realizations.Streetcode;
using Streetcode.DAL.Repositories.Realizations.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Realizations.Team;
using Streetcode.DAL.Repositories.Realizations.Timeline;
using Streetcode.DAL.Repositories.Realizations.Toponyms;
using Streetcode.DAL.Repositories.Realizations.Transactions;
using Streetcode.DAL.Repositories.Realizations.Users;

namespace Streetcode.DAL.Repositories.Realizations.Base;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly StreetcodeDbContext _streetcodeDbContext;

    private IVideoRepository _videoRepository;

    private IAudioRepository _audioRepository;

    private IStreetcodeCoordinateRepository _streetcodeCoordinateRepository;

    private IImageRepository _imageRepository;

    private IImageDetailsRepository _imageDetailsRepository;

    private IArtRepository _artRepository;

    private IStreetcodeArtRepository _streetcodeArtRepository;

    private IFactRepository _factRepository;

    private IPartnersRepository _partnersRepository;

    private ISourceCategoryRepository _sourceCategoryRepository;

    private IStreetcodeCategoryContentRepository _streetcodeCategoryContentRepository;

    private IRelatedFigureRepository _relatedFigureRepository;

    private IRelatedTermRepository _relatedTermRepository;

    private IStreetcodeRepository _streetcodeRepository;

    private ISubtitleRepository _subtitleRepository;

    private IStatisticRecordRepository _statisticRecordRepository;

    private ITagRepository _tagRepository;

    private ITermRepository _termRepository;

    private ITeamRepository _teamRepository;

    private IPositionRepository _positionRepository;

    private ITextRepository _textRepository;

    private ITimelineRepository _timelineRepository;

    private IToponymRepository _toponymRepository;

    private ITransactLinksRepository _transactLinksRepository;

    private IHistoricalContextRepository _historyContextRepository;

    private IPartnerSourceLinkRepository _partnerSourceLinkRepository;

    private IUserRepository _userRepository;

    private IStreetcodeTagIndexRepository _streetcodeTagIndexRepository;

    private IPartnerStreetcodeRepository _partnerStreetcodeRepository;

    private INewsRepository _newsRepository;

    private ITeamLinkRepository _teamLinkRepository;

    private ITeamPositionRepository _teamPositionRepository;

    private IHistoricalContextTimelineRepository _historicalContextTimelineRepository;

    private IStreetcodeToponymRepository _streetcodeToponymRepository;

    private IStreetcodeImageRepository _streetcodeImageRepository;

    public RepositoryWrapper(StreetcodeDbContext streetcodeDbContext)
    {
        _streetcodeDbContext = streetcodeDbContext;
    }

    public INewsRepository NewsRepository =>
          GetRepository<NewsRepository, News>(
      (NewsRepository)_newsRepository);

    public IFactRepository FactRepository =>
          GetRepository<FactRepository, Fact>(
      (FactRepository)_factRepository);

    public IImageRepository ImageRepository =>
          GetRepository<ImageRepository, Image>(
      (ImageRepository)_imageRepository);

    public ITeamRepository TeamRepository =>
          GetRepository<TeamRepository, TeamMember>(
      (TeamRepository)_teamRepository);

    public ITeamPositionRepository TeamPositionRepository =>
          GetRepository<TeamPositionRepository, TeamMemberPositions>(
      (TeamPositionRepository)_teamPositionRepository);

    public IAudioRepository AudioRepository =>
          GetRepository<AudioRepository, Audio>(
      (AudioRepository)_audioRepository);

    public IStreetcodeCoordinateRepository StreetcodeCoordinateRepository =>
          GetRepository<StreetcodeCoordinateRepository, StreetcodeCoordinate>(
      (StreetcodeCoordinateRepository)_streetcodeCoordinateRepository);

    public IVideoRepository VideoRepository =>
          GetRepository<VideoRepository, Video>(
      (VideoRepository)_videoRepository);

    public IArtRepository ArtRepository =>
          GetRepository<ArtRepository, Art>(
      (ArtRepository)_artRepository);

    public IStreetcodeArtRepository StreetcodeArtRepository =>
          GetRepository<StreetcodeArtRepository, StreetcodeArt>(
      (StreetcodeArtRepository)_streetcodeArtRepository);

    public IPartnersRepository PartnersRepository =>
          GetRepository<PartnersRepository, Partner>(
      (PartnersRepository)_partnersRepository);

    public ISourceCategoryRepository SourceCategoryRepository =>
          GetRepository<SourceCategoryRepository, SourceLinkCategory>(
      (SourceCategoryRepository)_sourceCategoryRepository);

    public IStreetcodeCategoryContentRepository StreetcodeCategoryContentRepository =>
          GetRepository<StreetcodeCategoryContentRepository, StreetcodeCategoryContent>(
      (StreetcodeCategoryContentRepository)_streetcodeCategoryContentRepository);

    public IRelatedFigureRepository RelatedFigureRepository =>
          GetRepository<RelatedFigureRepository, RelatedFigure>(
      (RelatedFigureRepository)_relatedFigureRepository);

    public IStreetcodeRepository StreetcodeRepository =>
          GetRepository<StreetcodeRepository, StreetcodeContent>(
      (StreetcodeRepository)_streetcodeRepository);

    public ISubtitleRepository SubtitleRepository =>
          GetRepository<SubtitleRepository, Subtitle>(
      (SubtitleRepository)_subtitleRepository);

    public IStatisticRecordRepository StatisticRecordRepository =>
          GetRepository<StatisticRecordsRepository, StatisticRecord>(
      (StatisticRecordsRepository)_statisticRecordRepository);

    public ITagRepository TagRepository =>
          GetRepository<TagRepository, Tag>(
      (TagRepository)_tagRepository);

    public ITermRepository TermRepository =>
          GetRepository<TermRepository, Term>(
      (TermRepository)_termRepository);

    public ITextRepository TextRepository =>
          GetRepository<TextRepository, Text>(
      (TextRepository)_textRepository);

    public ITimelineRepository TimelineRepository =>
          GetRepository<TimelineRepository, TimelineItem>(
      (TimelineRepository)_timelineRepository);
    
    public IToponymRepository ToponymRepository =>
          GetRepository<ToponymRepository, Toponym>(
      (ToponymRepository)_toponymRepository);

    public ITransactLinksRepository TransactLinksRepository =>
          GetRepository<TransactLinksRepository, TransactionLink>(
      (TransactLinksRepository)_transactLinksRepository);

    public IHistoricalContextRepository HistoricalContextRepository =>
          GetRepository<HistoricalContextRepository, HistoricalContext>(
      (HistoricalContextRepository)_historyContextRepository);

    public IPartnerSourceLinkRepository PartnerSourceLinkRepository =>
          GetRepository<PartnersourceLinksRepository, PartnerSourceLink>(
      (PartnersourceLinksRepository)_partnerSourceLinkRepository);
   
    public IRelatedTermRepository RelatedTermRepository =>
          GetRepository<RelatedTermRepository, RelatedTerm>(
      (RelatedTermRepository)_relatedTermRepository);
    
    public IUserRepository UserRepository =>
          GetRepository<UserRepository, User>(
      (UserRepository)_userRepository);

    public IStreetcodeTagIndexRepository StreetcodeTagIndexRepository =>
          GetRepository<StreetcodeTagIndexRepository, StreetcodeTagIndex>(
      (StreetcodeTagIndexRepository)_streetcodeTagIndexRepository);

    public IPartnerStreetcodeRepository PartnerStreetcodeRepository =>
        GetRepository<PartnerStreetodeRepository, StreetcodePartner>(
            (PartnerStreetodeRepository)_partnerStreetcodeRepository);

    public IPositionRepository PositionRepository => 
        GetRepository<PositionRepository, Positions>(
            (PositionRepository)_positionRepository);

    public ITeamLinkRepository TeamLinkRepository =>
          GetRepository<TeamLinkRepository, TeamMemberLink>(
            (TeamLinkRepository)_teamLinkRepository);
   
    public IImageDetailsRepository ImageDetailsRepository =>
        GetRepository<ImageDetailsRepository, ImageDetails>(
            (ImageDetailsRepository)_imageDetailsRepository);

    public IHistoricalContextTimelineRepository HistoricalContextTimelineRepository =>
        GetRepository<HistoricalContextTimelineRepository, HistoricalContextTimeline>(
            (HistoricalContextTimelineRepository)_historicalContextTimelineRepository);

    public IStreetcodeToponymRepository StreetcodeToponymRepository =>
        GetRepository<StreetcodeToponymRepository, StreetcodeToponym>(
            (StreetcodeToponymRepository)_streetcodeToponymRepository);

    public IStreetcodeImageRepository StreetcodeImageRepository =>
        GetRepository<StreetcodeImageRepository, StreetcodeImage>(
            (StreetcodeImageRepository)_streetcodeImageRepository);

    public T GetRepository<T, TU>(T repo)
      where T : RepositoryBase<TU>, new()
      where TU : class, new()
    {
        if (repo is null)
        {
            repo = new T();
            repo.DbContext = _streetcodeDbContext;
        }

        return repo;
    }

    public int SaveChanges()
    {
        return _streetcodeDbContext.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _streetcodeDbContext.SaveChangesAsync();
    }

    public TransactionScope BeginTransaction()
    {
        return new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
    }
}
