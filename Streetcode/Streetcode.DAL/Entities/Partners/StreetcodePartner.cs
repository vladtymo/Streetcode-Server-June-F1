using System.ComponentModel.DataAnnotations;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Entities.Partners
{
    public class StreetcodePartner
    {
        public int StreetcodeId { get; set; }

        public int PartnerId { get; set; }

        public StreetcodeContent Streetcode { get; set; } = new();

        public Partner Partner { get; set; } = new();
    }
}
