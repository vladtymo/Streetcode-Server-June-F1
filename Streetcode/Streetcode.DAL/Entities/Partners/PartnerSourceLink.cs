using Streetcode.DAL.Entities.Base;
using Streetcode.DAL.Enums;

namespace Streetcode.DAL.Entities.Partners;

public class PartnerSourceLink : IEntityId<int>
{
    public int Id { get; set; }

    public LogoType LogoType { get; set; }

    public string? TargetUrl { get; set; } = string.Empty;

    public int PartnerId { get; set; }

    public Partner? Partner { get; set; }
}