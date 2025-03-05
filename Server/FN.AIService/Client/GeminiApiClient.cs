using FN.Application.Catalog.Blogs;
using Ganss.Xss;
using GeminiAIDev.Models;
using GeminiAIDev.Models.ContentResponse;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace GeminiAIDev.Client
{
    public class GeminiApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly GameTitleExtractor _titleExtractor;
        private readonly IImageSearchService _imageSearch;
        private readonly string _apiKey;
        public GeminiApiClient(GameTitleExtractor titleExtractor, IImageSearchService imageSearch, string apiKey)
        {
            _httpClient = new HttpClient();
            _apiKey = apiKey;
            _imageSearch = imageSearch;
            _titleExtractor = titleExtractor;
        }
        public async Task<string> GenerateContentAsync(string prompt)
        {
            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_apiKey}";
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
        public async Task<PostResponse> CreatePostAsync()
        {
            // Gọi Gemini API
            var geminiResponse = await GetGeminiResponse();

            // Xử lý response
            var postData = ProcessGeminiResponse(geminiResponse);

            // Trích xuất tên game CHÍNH XÁC
            var gameTitle = await _titleExtractor.ExtractGameTitleAsync(postData.DetailHtml);

            // Tìm thumbnail
            postData.Thumbnail = await _imageSearch.GetGameThumbnailAsync(gameTitle);

            return postData;
        }
        private async Task<ContentResponse> GetGeminiResponse()
        {
            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_apiKey}";

            // Tạo prompt yêu cầu Gemini trả về HTML
            var prompt = "Hãy tạo bài viết về tin tức game với:\n" +
                         "1. Title: Tiêu đề hấp dẫn\n" +
                         "2. Thumbnail: URL ảnh hợp lệ\n" +
                         "3. Description: Mô tả ngắn\n" +
                         "4. DetailHtml: Nội dung chi tiết ĐƯỢC ĐỊNH DẠNG HTML\n" +
                         "Ví dụ:\n" +
                         "{\n" +
                         "  \"Title\": \"Elden Ring DLC Shadow of the Erdtree Phá Kỷ Lục Doanh Thu\",\n" +
                         "  \"Thumbnail\": \"https://example.com/elden-ring-dlc.jpg\",\n" +
                         "  \"Description\": \"DLC được mong đợi nhất 2024 chính thức ra mắt\",\n" +
                         "  \"DetailHtml\": \"<h2>Cập Nhật Gameplay</h2><p>Blizzard đã công bố...</p>\"\n" +
                         "}";

            var contentRequest = new ContentRequest
            {
                contents = new[]
                {
            new Models.Content
            {
                parts = new[]
                {
                    new Models.Part { text = prompt }
                }
            }
        }
            };

            string jsonRequest = JsonConvert.SerializeObject(contentRequest);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Lỗi khi gọi Gemini API: {response.StatusCode}");
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ContentResponse>(jsonResponse);
        }
        private PostResponse ProcessGeminiResponse(ContentResponse response)
        {
            if (response?.Candidates == null || response.Candidates.Length == 0)
            {
                throw new Exception("Không có dữ liệu trả về từ Gemini");
            }

            // Lấy nội dung từ response
            var geminiOutput = response.Candidates[0].Content.Parts[0].Text;

            // Loại bỏ các ký tự không cần thiết (```json và ```)
            geminiOutput = geminiOutput.Replace("```json", "").Replace("```", "").Trim();

            // Phân tích JSON
            var postData = JsonConvert.DeserializeObject<PostResponse>(geminiOutput);

            // Validate các trường bắt buộc
            if (string.IsNullOrEmpty(postData.Title))
            {
                throw new Exception("Tiêu đề không được để trống");
            }

            if (string.IsNullOrEmpty(postData.DetailHtml))
            {
                throw new Exception("Nội dung chi tiết không được để trống");
            }

            // Sanitize HTML
            var sanitizer = new HtmlSanitizer();
            postData.DetailHtml = sanitizer.Sanitize(postData.DetailHtml);

            // Fallback cho Description nếu trống
            if (string.IsNullOrEmpty(postData.Description))
            {
                postData.Description = GenerateDescriptionFromContent(postData.DetailHtml);
            }

            return postData;
        }

        private string GenerateDescriptionFromContent(string htmlContent)
        {
            // Loại bỏ HTML tags
            var plainText = Regex.Replace(htmlContent, "<.*?>", string.Empty);

            // Lấy 2 câu đầu tiên
            var sentences = plainText.Split('.', StringSplitOptions.RemoveEmptyEntries);
            return sentences.Length > 1
                   ? $"{sentences[0].Trim()}. {sentences[1].Trim()}."
                   : plainText.Length > 100
                     ? plainText.Substring(0, 100) + "..."
                     : plainText;
        }
        #region[Fail]
        //public async Task<PostResponse> CreatePostAsync()
        //{
        //    string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_apiKey}";

        //    string prompt = $"Hãy tạo bài viết về tin tức game trong ngày {DateTime.UtcNow.ToShortDateString} với:\n" +
        //        "1. Title: Tiêu đề hấp dẫn\n" +
        //       "2. Thumbnail: URL ảnh hợp lệ\n" +
        //       "3. Description: Mô tả ngắn\n" +
        //       "4. DetailHtml: Nội dung dài, tối thiểu 1000 chữ, chi tiết ĐƯỢC ĐỊNH DẠNG HTML với:\n" +
        //       "   - Thẻ <h2> cho tiêu đề phụ\n" +
        //       "   - Thẻ <ul>/<li> cho danh sách\n" +
        //       "   - Thẻ <p> cho đoạn văn\n" +
        //       "   - Thẻ <strong> cho text quan trọng\n" +
        //       "   - Không sử dụng CSS inline\n" +
        //       "Ví dụ output HTML:\n" +
        //       "<h2>Cập Nhật Gameplay</h2>\n" +
        //       "<p>Blizzard đã công bố...</p>\n" +
        //       "<ul>\n" +
        //       "  <li><strong>Tempering:</strong> Cho phép tùy chỉnh</li>\n" +
        //       "  <li><strong>Helltide:</strong> Sự kiện mới</li>\n" +
        //       "</ul>" +
        //       "Hãy trả về kết quả dưới dạng JSON với các trường: Title, Thumbnail, Description, DetailHtml.";
        //    var contentRequest = new ContentRequest
        //    {
        //        contents = new[]
        //            {
        //            new Models.Content
        //            {
        //                parts = new[]
        //                {
        //                    new Models.Part { text = prompt }
        //                }
        //            }
        //        }
        //    };

        //    string jsonRequest = JsonConvert.SerializeObject(contentRequest);
        //    var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        //    HttpResponseMessage response = await _httpClient.PostAsync(url, content);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        string jsonResponse = await response.Content.ReadAsStringAsync();
        //        var geminiResponse = JsonConvert.DeserializeObject<ContentResponse>(jsonResponse);

        //        // Trích xuất và làm sạch JSON
        //        string geminiOutput = geminiResponse.Candidates[0].Content.Parts[0].Text;
        //        geminiOutput = geminiOutput.Replace("```json", "").Replace("```", "").Trim();

        //        // Phân tích JSON
        //        var postData = JsonConvert.DeserializeObject<PostResponse>(geminiOutput);

        //        var gameTitle = ExtractGameTitle(postData.Title);
        //        postData.Thumbnail = await GetGameThumbnail(gameTitle);
        //        postData.DetailHtml = postData.DetailHtml.Replace("\n", "").Replace("  ", " ");
        //        return postData;
        //    }
        //    else
        //    {
        //        throw new Exception("Error communicating with Gemini API.");
        //    }
        //}
        //private async Task<string> GetGameThumbnail(string gameTitle)
        //{
        //    string apiKey = "AIzaSyDCSTJGV6do4B_AxPQRa4bUXpDrF5pkg2o";
        //    string cx = "818b7a1aa0fa94c37";
        //    string url = $"https://www.googleapis.com/customsearch/v1?q={Uri.EscapeDataString(gameTitle)}+game+cover&key={apiKey}&cx={cx}&searchType=image";

        //    var response = await _httpClient.GetStringAsync(url);
        //    dynamic result = JsonConvert.DeserializeObject(response);
        //    Random random = new Random();

        //    return result.items[random.Next(0, 5)].link.ToString();
        //}
        //public class DefaultThumbnails
        //{
        //    private static readonly string[] GameThumbnails =
        //    {
        //        "/images/default/gaming1.jpg",
        //        "/images/default/gaming2.jpg",
        //        "/images/default/gaming3.jpg"
        //    };

        //    public static string GetRandomDefault() => GameThumbnails[new Random().Next(GameThumbnails.Length)];
        //}
        //private string ExtractGameTitle(string title)
        //{
        //    // Triển khai logic trích xuất tên game từ nội dung
        //    var match = Regex.Match(title, @"\b(Diablo IV|Cyberpunk 2077|Elden Ring|GTA|Genshin Impact)\b", RegexOptions.IgnoreCase);
        //    return match.Success ? match.Value : "Game";
        //}
        #endregion
    }
}