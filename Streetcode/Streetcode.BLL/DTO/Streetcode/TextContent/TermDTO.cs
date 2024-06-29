using Streetcode.DAL.Entities.Streetcode.TextContent;

namespace Streetcode.BLL.DTO.Streetcode.TextContent;

public class TermDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<RelatedTerm>? RelatedTerms { get; set; }
}