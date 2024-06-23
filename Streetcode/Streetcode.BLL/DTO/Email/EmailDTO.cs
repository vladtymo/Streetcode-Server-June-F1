using System.ComponentModel.DataAnnotations;

namespace Streetcode.BLL.DTO.Email
{
    public class EmailDTO
    {
        public string From { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
    }
}
