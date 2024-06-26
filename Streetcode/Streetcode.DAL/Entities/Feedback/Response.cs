using Streetcode.DAL.Entities.Base;

namespace Streetcode.DAL.Entities.Feedback;

public class Response : IEntityId
{
    public int Id { get; set; }

    public string? Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Description { get; set; } = string.Empty;
}