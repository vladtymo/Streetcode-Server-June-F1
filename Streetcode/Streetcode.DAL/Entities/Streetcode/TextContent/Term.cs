using Streetcode.DAL.Entities.Base;

namespace Streetcode.DAL.Entities.Streetcode.TextContent;

public class Term : IEntityId<int>
{
    public int Id { get; set; }

    public string? Title { get; set; } = string.Empty;

    public string? Description { get; set; } = string.Empty;

    public List<RelatedTerm> RelatedTerms { get; set; } = new ();
}