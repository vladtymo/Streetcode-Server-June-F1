namespace Streetcode.BLL.DTO.Comment
{
    public class CommentDTO
    {
        public int Id { get; set; } 
        public string? CommentContent { get; set; }
        public int StreetcodeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
    }
}
