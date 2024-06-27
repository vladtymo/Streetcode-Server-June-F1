using Streetcode.DAL.Entities.Base;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Entities.Transactions;

public class TransactionLink : IEntityId<int>
{
    public int Id { get; set; }

    public string? UrlTitle { get; set; }

    public string? Url { get; set; }

    public int StreetcodeId { get; set; }

    public StreetcodeContent? Streetcode { get; set; }
}