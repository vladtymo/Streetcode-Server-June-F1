using Streetcode.DAL.Entities.Base;
using Newtonsoft.Json;

namespace Streetcode.DAL.Entities.Payment
{
    public class MerchantPaymentInfo : IEntity
    {
        // [JsonProperty("reference")]
        // public string Reference { get; set; }

        [JsonProperty("destination")]
        public string Destination { get; set; } = string.Empty;

        // [JsonProperty("basketOrder")]
        // public List<BasketOrder> BasketOrder { get; set; }
    }
}