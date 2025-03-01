using GeminiAIDev.Models;
using GeminiAIDev.Models.ContentResponse;
using Newtonsoft.Json;
using System.Text;

namespace GeminiAIDev.Client
{
    public class GeminiApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        public GeminiApiClient(string apiKey)
        {
            _httpClient = new HttpClient();
            _apiKey = apiKey;
        }
        public async Task<string> GenerateContentAsync(string prompt)
        {
            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_apiKey}";
            var request = new ContentRequest
            {
                contents = new[]
                {
                    new Models.Content
                    {
                        parts = new[]
                        {
                            new Models.Part
                            {
                                text = prompt
                            }
                        }
                    }
                }
            };
            string jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                // You can deserialize jsonResponse if needed
                var geminiResponse = JsonConvert.DeserializeObject<ContentResponse>(jsonResponse);
                return geminiResponse.Candidates[0].Content.Parts[0].Text;
            }
            else
            {
                throw new Exception("Error communicating with Gemini API.");
            }
        }
    }
}
