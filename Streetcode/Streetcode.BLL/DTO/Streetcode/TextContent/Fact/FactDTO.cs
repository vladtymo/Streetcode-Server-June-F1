using System.ComponentModel.DataAnnotations;

namespace Streetcode.BLL.DTO.Streetcode.TextContent.Fact;

public class FactDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int ImageId { get; set; }
    public string FactContent { get; set; } = string.Empty;
    public int StreetcodeId { get; set; }
    public int Position { get; set; }
}
