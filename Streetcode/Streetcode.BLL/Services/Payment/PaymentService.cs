using System.Net.Mime;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Streetcode.BLL.Interfaces.Payment;
using Streetcode.BLL.Services.Payment.Exceptions;
using Streetcode.BLL.Services.Payment.PaymentEnviroment;
using Streetcode.DAL.Entities.Payment;

namespace Streetcode.BLL.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly PaymentEnvirovmentVariables _paymentEnvirovment;
        private readonly HttpClient _httpClient;
        private readonly string _createInvoice;
        public PaymentService(IOptions<PaymentEnvirovmentVariables> paymentEnvirovment, IHttpClientFactory httpClientFactory)
        {
            _paymentEnvirovment = paymentEnvirovment.Value;
            _createInvoice = _paymentEnvirovment.Api.Merchant.Invoice.Create;
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("PaymentClient");
        }

        public async Task<InvoiceInfo> CreateInvoiceAsync(DAL.Entities.Payment.Invoice invoice)
        {
            var (code, body) = await PostAsync(_createInvoice, invoice);

            return code switch
            {
                200 => JsonToObject<InvoiceInfo>(body),
                400 => throw new InvalidRequestParameterException(JsonToObject<Error>(body)),
                403 => throw new InvalidTokenException(),
                _ => throw new NotSupportedException()
            };
        }

        private async Task<(int Code, string Body)> PostAsync<T>(string url, T data)
        {
                var jsonString = JsonConvert.SerializeObject(data, Formatting.None);
                var content = new StringContent(jsonString, Encoding.UTF8, MediaTypeNames.Application.Json);
                var response = await _httpClient.PostAsync(url, content);
                return (
                    Code: (int)response.StatusCode,
                    Body: await response.Content.ReadAsStringAsync());
        }

        private T JsonToObject<T>(string body)
        {
            var result = JsonConvert.DeserializeObject<T>(body);

            return result ?? throw new InvalidOperationException($"Failed to deserialize JSON to {typeof(T).Name}");
        }
    }
}
