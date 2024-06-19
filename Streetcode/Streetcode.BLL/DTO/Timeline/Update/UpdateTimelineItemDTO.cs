using System.ComponentModel.DataAnnotations;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Timeline.Update
{
    public class UpdateTimelineItemDTO
    {
        public int Id { get; set; }

        [MaxLength(26)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(400)]
        public string Description { get; set; } = string.Empty;

        public DateTime Date { get; set; }
        public DateViewPattern DateViewPattern { get; set; }

        public List<HistoricalContextDTO>? HistoricalContexts { get; set; } = new List<HistoricalContextDTO>();
    }
}
