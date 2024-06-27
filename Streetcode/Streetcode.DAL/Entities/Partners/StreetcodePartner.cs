using Streetcode.DAL.Entities.Base;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Entities.Partners
{
    public class StreetcodePartner : IEntity
    {
        public int StreetcodeId { get; set; }

        public int PartnerId { get; set; }

        public StreetcodeContent? Streetcode { get; set; } 

        public Partner? Partner { get; set; }
    }
}
