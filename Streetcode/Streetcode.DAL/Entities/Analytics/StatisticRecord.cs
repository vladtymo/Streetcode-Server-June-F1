using Streetcode.DAL.Entities.Base;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Entities.Analytics
{
    public class StatisticRecord : IEntityId<int>
    {
        public int Id { get; set; }
        public int QrId { get; set; }
        public int Count { get; set; }

        public string Address { get; set; } = string.Empty;

        public int StreetcodeId { get; set; }
        public StreetcodeContent? Streetcode { get; set; }

        public int StreetcodeCoordinateId { get; set; }
        public StreetcodeCoordinate StreetcodeCoordinate { get; set; }
     }
}
