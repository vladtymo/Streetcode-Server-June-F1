using System.Runtime;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Streetcode.BLL.Interfaces.Instagram;
using Streetcode.DAL.Entities.Instagram;

namespace Streetcode.BLL.Services.Instagram
{
    public class InstagramService : IInstagramService
    {
        private readonly HttpClient _httpClient;
        private readonly InstagramEnvirovmentVariables _envirovment;
        private readonly string _userId;
        private readonly string _accessToken;
        private static int postLimit = 10;

        public InstagramService(IHttpClientFactory httpFactory, IOptions<InstagramEnvirovmentVariables> instagramEnvirovment)
        {
            _httpClient = httpFactory.CreateClient("InstagramClient");
            _envirovment = instagramEnvirovment.Value;
            _userId = _envirovment.InstagramID;
            _accessToken = _envirovment.InstagramToken;
        }

        public async Task<IEnumerable<InstagramPost>> GetPostsAsync()
        {
            string apiUrl = _envirovment.MediaRequestUrl
            .Replace("{InstagramID}", _userId)
            .Replace("{InstagramToken}", _accessToken)
            .Replace("{postLimit}", (2 * postLimit).ToString());

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var postResponse = JsonSerializer.Deserialize<InstagramPostResponse>(jsonResponse, jsonOptions);

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
    }
}