namespace Streetcode.DAL.Entities.Comments;

public class Reply : Comment
{
    public int? ParentId { get; set; }
    public Comment? ParentComment { get; set; }
    public List<Comment>? Replies { get; set; } = new List<Comment>();
}