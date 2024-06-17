using System.ComponentModel.DataAnnotations;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Timeline.Create;

public class CreateTimelineItemDTO
{
    [MaxLength(26)]
    public string? Title { get; set; }
    [MaxLength(400)]
    public string? Description { get; set; }
    
    public DateTime Date { get; set; }
    public DateViewPattern DateViewPattern { get; set; }

    public List<HistoricalContextDTO>? HistoricalContexts { get; set; } = new List<HistoricalContextDTO>();
    
    [Required]
    public int StreetCodeId { get; set; }
}