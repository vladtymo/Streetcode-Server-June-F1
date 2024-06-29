using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Streetcode.BLL.Interfaces.Instagram;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources;
using Streetcode.DAL.Entities.Instagram;

namespace Streetcode.BLL.Services.Instagram
{
    public class InstagramService : IInstagramService
    {
        private readonly HttpClient _httpClient;
        private readonly InstagramEnvirovmentVariables _envirovment;
        private readonly ILoggerService _logger;
        private readonly string _userId;
        private readonly string _accessToken;
        private static int postLimit = 10;

        public InstagramService(IHttpClientFactory httpFactory, IOptions<InstagramEnvirovmentVariables> instagramEnvirovment, ILoggerService logger)
        {
            _httpClient = httpFactory.CreateClient("InstagramClient");
            _envirovment = instagramEnvirovment.Value;
            _logger = logger;
            _userId = _envirovment.InstagramID;
            _accessToken = _envirovment.InstagramToken;
        }

        public async Task<IEnumerable<InstagramPost>> GetPostsAsync()
        {
            var apiUrlBuilder = new StringBuilder(_envirovment.MediaRequestUrl);
            apiUrlBuilder.Replace("{InstagramID}", _userId)
                         .Replace("{InstagramToken}", _accessToken)
                         .Replace("{postLimit}", (2 * postLimit).ToString());

            string apiUrl = apiUrlBuilder.ToString();

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            string jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                int statusCode = Convert.ToInt32(response.StatusCode);
                string erorrMsg = string.Format(ErrorMessages.BadRequestToInstagram, statusCode,jsonResponse);
                _logger.LogError(response, erorrMsg);
                throw new HttpRequestException(erorrMsg);
            }

            var postResponse = DeserializeResponse<InstagramPostResponse>(jsonResponse);
            IEnumerable<InstagramPost> posts = new List<InstagramPost>();

            if (postResponse?.Data != null)
            {
                posts = RemoveVideoMediaType(postResponse.Data);
            }

            return posts;
        }

        public IEnumerable<InstagramPost> RemoveVideoMediaType(IEnumerable<InstagramPost> posts)
        {
            return posts.Where(p => p.MediaType != "VIDEO").Take(postLimit);
        }

        private T? DeserializeResponse<T>(string jsonResponse)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            return JsonSerializer.Deserialize<T>(jsonResponse, jsonOptions);
        }
    }
}
