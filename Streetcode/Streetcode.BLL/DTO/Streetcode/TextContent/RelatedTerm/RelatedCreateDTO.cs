using Streetcode.BLL.DTO.Streetcode.TextContent.Term;

namespace Streetcode.BLL.DTO.Streetcode.TextContent.RelatedTerm;

public class RelatedTermCreateDTO
{
    public string Word { get; set; } = string.Empty;
    public int TermId { get; set; }
    public TermDTO? Term { get; set; }
}