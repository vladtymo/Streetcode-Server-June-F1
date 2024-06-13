using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Analytics;
using Streetcode.DAL.Entities.Feedback;
using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Entities.Streetcode.Types;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Entities.Transactions;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Enums;

namespace Streetcode.DAL.Persistence;

public class StreetcodeDbContext : DbContext
{
    public StreetcodeDbContext()
    {
    }

    public StreetcodeDbContext(DbContextOptions<StreetcodeDbContext> options)
        : base(options)
    {
    }

    public DbSet<Art> Arts { get; set; }
    public DbSet<Audio> Audios { get; set; }
    public DbSet<ToponymCoordinate> ToponymCoordinates { get; set; }
    public DbSet<StreetcodeCoordinate> StreetcodeCoordinates { get; set; }
    public DbSet<Fact> Facts { get; set; }
    public DbSet<HistoricalContext> HistoricalContexts { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<ImageDetails> ImageDetailses { get; set; }
    public DbSet<Partner> Partners { get; set; }
    public DbSet<PartnerSourceLink> PartnerSourceLinks { get; set; }
    public DbSet<RelatedFigure> RelatedFigures { get; set; }
    public DbSet<Response> Responses { get; set; }
    public DbSet<StreetcodeContent> Streetcodes { get; set; }
    public DbSet<Subtitle> Subtitles { get; set; }
    public DbSet<StatisticRecord> StatisticRecords { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Term> Terms { get; set; }
    public DbSet<RelatedTerm> RelatedTerms { get; set; }
    public DbSet<Text> Texts { get; set; }
    public DbSet<TimelineItem> TimelineItems { get; set; }
    public DbSet<Toponym> Toponyms { get; set; }
    public DbSet<TransactionLink> TransactionLinks { get; set; }
    public DbSet<Video> Videos { get; set; }
    public DbSet<StreetcodeCategoryContent> StreetcodeCategoryContent { get; set; }
    public DbSet<StreetcodeArt> StreetcodeArts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<StreetcodeTagIndex> StreetcodeTagIndices { get; set; }
    public DbSet<TeamMember> TeamMembers { get; set; }
    public DbSet<TeamMemberLink> TeamMemberLinks { get; set; }
    public DbSet<Positions> Positions { get; set; }
    public DbSet<News> News { get; set; }
    public DbSet<SourceLinkCategory> SourceLinks { get; set; }
    public DbSet<StreetcodeImage> StreetcodeImages { get; set; }
    public DbSet<HistoricalContextTimeline> HistoricalContextsTimelines { get; set; }
    public DbSet<StreetcodePartner> StreetcodePartners { get; set; }
    public DbSet<TeamMemberPositions> TeamMemberPosition { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=StreetcodeDB;Integrated Security=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.UseCollation("SQL_Ukrainian_CP1251_CI_AS");

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(StreetcodeDbContext).Assembly);

        modelBuilder.Entity<News>()
            .HasOne(x => x.Image)
            .WithOne(x => x.News)
            .HasForeignKey<News>(x => x.ImageId);

        modelBuilder.Entity<TeamMember>()
            .HasOne(x => x.Image)
            .WithOne(x => x.TeamMember)
            .HasForeignKey<TeamMember>(x => x.ImageId);

        modelBuilder.Entity<TeamMember>()
            .HasMany(x => x.Positions)
            .WithMany(x => x.TeamMembers)
            .UsingEntity<TeamMemberPositions>(
            tp => tp.HasOne(x => x.Positions).WithMany().HasForeignKey(x => x.PositionsId),
            tp => tp.HasOne(x => x.TeamMember).WithMany().HasForeignKey(x => x.TeamMemberId));

        modelBuilder.Entity<TeamMember>()
            .HasMany(x => x.TeamMemberLinks)
            .WithOne(x => x.TeamMember)
            .HasForeignKey(x => x.TeamMemberId);

        modelBuilder.Entity<TeamMemberPositions>()
            .HasKey(nameof(TeamMemberPositions.TeamMemberId), nameof(TeamMemberPositions.PositionsId));

        modelBuilder.Entity<Partner>(entity =>
        {
            entity.HasMany(d => d.PartnerSourceLinks)
                .WithOne(p => p.Partner)
                .HasForeignKey(d => d.PartnerId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(p => p.IsKeyPartner)
                .HasDefaultValue("false");
        });

        modelBuilder.Entity<HistoricalContextTimeline>()
             .HasKey(ht => new { ht.TimelineId, ht.HistoricalContextId });
        modelBuilder.Entity<HistoricalContextTimeline>()
            .HasOne(ht => ht.Timeline)
            .WithMany(x => x.HistoricalContextTimelines)
            .HasForeignKey(x => x.TimelineId);
        modelBuilder.Entity<HistoricalContextTimeline>()
            .HasOne(ht => ht.HistoricalContext)
            .WithMany(x => x.HistoricalContextTimelines)
            .HasForeignKey(x => x.HistoricalContextId);

        modelBuilder.Entity<SourceLinkCategory>()
            .HasMany(d => d.StreetcodeCategoryContents)
            .WithOne(p => p.SourceLinkCategory)
            .HasForeignKey(d => d.SourceLinkCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasOne(d => d.Art)
                .WithOne(a => a.Image)
                .HasForeignKey<Art>(a => a.ImageId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(im => im.ImageDetails)
                .WithOne(info => info.Image)
                .HasForeignKey<ImageDetails>(a => a.ImageId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Partner)
                .WithOne(p => p.Logo)
                .HasForeignKey<Partner>(d => d.LogoId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(d => d.Facts)
                .WithOne(p => p.Image)
                .HasForeignKey(d => d.ImageId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(i => i.SourceLinkCategories)
                .WithOne(s => s.Image)
                .HasForeignKey(d => d.ImageId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<StreetcodeArt>(entity =>
        {
            entity.HasKey(d => new { d.ArtId, d.StreetcodeId });

            entity.HasOne(d => d.Streetcode)
                .WithMany(d => d.StreetcodeArts)
                .HasForeignKey(d => d.StreetcodeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Art)
                .WithMany(d => d.StreetcodeArts)
                .HasForeignKey(d => d.ArtId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.Index)
                .HasDefaultValue(1);

            entity
                .HasIndex(d => new { d.ArtId, d.StreetcodeId })
                .IsUnique(false);
        });
    }
}
