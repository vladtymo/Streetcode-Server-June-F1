using System.ComponentModel.DataAnnotations;

namespace Streetcode.BLL.DTO.Sources.Create
{
    public class CreateSourceCategoryDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ImageId { get; set; }
    }
}