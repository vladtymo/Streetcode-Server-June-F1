using System.ComponentModel.DataAnnotations;

namespace Streetcode.BLL.DTO.Streetcode.TextContent.Fact
{
    public class FactUpdateCreateDto : FactDto
    {
        [MaxLength(200)]
        public string? ImageDescription { get; set; }
    }
}