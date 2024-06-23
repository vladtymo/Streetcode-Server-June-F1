using System.ComponentModel.DataAnnotations;

namespace Streetcode.BLL.DTO.Media.Images
{
    public class ImageDetailsDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Alt { get; set; } = string.Empty;

        public int ImageId { get; set; }
    }
}
