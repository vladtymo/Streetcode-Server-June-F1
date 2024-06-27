using Streetcode.DAL.Entities.Base;
using Newtonsoft.Json;

namespace Streetcode.DAL.Entities.Payment
{
    public class SaveCardData : IEntity
    {
        [JsonProperty("saveCard")]
        public bool SaveCard { get; set; }

        [JsonProperty("walletId")]
        public string WalletId { get; set; } = string.Empty;
    }
}