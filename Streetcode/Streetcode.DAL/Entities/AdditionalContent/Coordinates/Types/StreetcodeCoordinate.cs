using Streetcode.DAL.Entities.Analytics;
using Streetcode.DAL.Entities.Base;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;

public class StreetcodeCoordinate : Coordinate, IEntity
{
    public int StreetcodeId { get; set; }

    public StreetcodeContent? Streetcode { get; set; }

    public StatisticRecord? StatisticRecord { get; set; }
}
