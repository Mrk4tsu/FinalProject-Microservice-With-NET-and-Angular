
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;

namespace FN.Application.Catalog.Blogs
{
    public class ImageSearchService : IImageSearchService
    {
        private readonly HttpClient _httpClient;
        private readonly string _googleApiKey;
        private readonly string _googleCx;
        private readonly string _unsplashAccessKey;
        public ImageSearchService(
        IConfiguration config,
        HttpClient httpClient)
        {
            _httpClient = httpClient;
            _googleApiKey = config["ApiKeys:GoogleApiKey"];
            _googleCx = config["ApiKeys:GoogleCx"];
            _unsplashAccessKey = config["ApiKeys:UnsplashAccessKey"];
        }
        public async Task<string> GetGameThumbnailAsync(string query)
        {
            try
            {
                // Thử Google Custom Search trước
                var googleUrl = $"https://www.googleapis.com/customsearch/v1?q={WebUtility.UrlEncode(query)}&key={_googleApiKey}&cx={_googleCx}&searchType=image";
                var googleResponse = await _httpClient.GetStringAsync(googleUrl);
                var googleResult = JsonConvert.DeserializeObject<GoogleImageResponse>(googleResponse);

                if (googleResult?.Items?.Count > 0 && await IsValidImage(googleResult.Items[0].Link))
                {
                    return googleResult.Items[0].Link;
                }

                // Fallback sang Unsplash
                //var unsplashUrl = $"https://api.unsplash.com/search/photos?page=1&query={WebUtility.UrlEncode(query)}&client_id={_unsplashAccessKey}";
                //var unsplashResponse = await _httpClient.GetStringAsync(unsplashUrl);
                //var unsplashResult = JsonConvert.DeserializeObject<UnsplashResponse>(unsplashResponse);

                //if (unsplashResult?.Results?.Count > 0)
                //{
                //    return unsplashResult.Results[0].Urls.Regular;
                //}

                return GetDefaultThumbnail();
            }
            catch
            {
                return GetDefaultThumbnail();
            }
        }
        private async Task<bool> IsValidImage(string url)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Head, url);
                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode &&
                       response.Content.Headers.ContentType?.MediaType?.StartsWith("image/") == true;
            }
            catch
            {
                return false;
            }
        }

        private string GetDefaultThumbnail() => "https://mrkatsu.io.vn/default-thumbnail.jpg";
        private class GoogleImageResponse
        {
            public List<GoogleImageItem> Items { get; set; }
        }

        private class GoogleImageItem
        {
            public string Link { get; set; }
        }

        private class UnsplashResponse
        {
            public List<UnsplashResult> Results { get; set; }
        }

        private class UnsplashResult
        {
            public UnsplashUrls Urls { get; set; }
        }

        private class UnsplashUrls
        {
            public string Regular { get; set; }
        }
    }
}
