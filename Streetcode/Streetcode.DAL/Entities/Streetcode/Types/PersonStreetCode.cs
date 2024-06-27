using Streetcode.DAL.Entities.Base;

namespace Streetcode.DAL.Entities.Streetcode.Types;

public class PersonStreetcode : StreetcodeContent, IEntity
{
    public string FirstName { get; set; } = string.Empty;

    public string? Rank { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;
}