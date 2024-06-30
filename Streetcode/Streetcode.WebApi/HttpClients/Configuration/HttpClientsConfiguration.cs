using System.Net;
using System.Net.Security;
using Streetcode.BLL.Resources;
using Streetcode.BLL.Services.Instagram;
using Streetcode.BLL.Services.Payment.PaymentEnviroment;
using Streetcode.WebApi.HttpClients.Handlers;
using Streetcode.WebApi.HttpClients.Policies;

namespace Streetcode.WebApi.HttpClients.Configuration;

public static class HttpClientsConfiguration
{
    public static void AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("InstagramClient", client =>
        {
            var settings = configuration.GetSection("Instagram").Get<InstagramEnvirovmentVariables>();

            if(settings == null)
            {
                throw new InvalidOperationException(ErrorMessages.EmptyInstagramSettings);
            }

            client.BaseAddress = new Uri(settings.BaseAddress);
        });

        services.AddHttpClient("PaymentClient", client =>
        {
            var settings = configuration.GetSection("Payment").Get<PaymentEnvirovmentVariables>();
            if (settings == null)
            {
                throw new InvalidOperationException(ErrorMessages.EmptyPaymentSettings);
            }

            client.BaseAddress = new Uri(settings.Api.Production);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add(settings.XToken, settings.Token);
        });

        services.AddHttpClient("DownloadFileClient", client =>
        {
            client.Timeout = TimeSpan.FromSeconds(60);
        })
         .ConfigurePrimaryHttpMessageHandler(() =>
         {
             var clientHandler = new HttpClientHandler();
             clientHandler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
             clientHandler.ServerCertificateCustomValidationCallback += (message, cert, chain, errors) =>
             {
                 // Anything that would have been accepted by default is OK
                 if (errors == SslPolicyErrors.None)
                 {
                     return true;
                 }

                 // If there is something wrong other than a chain processing error, don't trust it.
                 if (errors != SslPolicyErrors.RemoteCertificateChainErrors || chain.ChainStatus.Length == 0)
                 {
                     return false;
                 }

                 return true;
             };
             return clientHandler;
         })
          .AddHttpMessageHandler(() => new PolicyHandler(PolicyProvider.GetRetryPolicy()))
          .AddHttpMessageHandler(() => new PolicyHandler(PolicyProvider.GetCircuitBreakerPolicy()));

        services.AddHttpClient("FetchCoordsClient", client =>
        { 
            // Add user-agent and referer headers to request
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            client.DefaultRequestHeaders.Add("Referer", "http://www.microsoft.com");
        })
          .AddHttpMessageHandler(() => new PolicyHandler(PolicyProvider.GetRetryPolicy()))
          .AddHttpMessageHandler(() => new PolicyHandler(PolicyProvider.GetCircuitBreakerPolicy()));
    }
}
