using System.Text;
using System.Text.RegularExpressions;

namespace FN.Application.Catalog.Blogs
{
    public class GameTitleExtractor
    {
        private readonly HttpClient _httpClient;
        private readonly HashSet<string> _gameKeywords;
        private readonly Regex _titleRegex;

        public GameTitleExtractor(HttpClient httpClient)
        {
            _httpClient = httpClient;

            // Khởi tạo regex cho tên game
            _titleRegex = new Regex(@"
            (?<![a-zA-Z0-9])
            (
                (?:[A-Z][a-z']+\s*)+  
                |
                (?:\b[A-Z0-9]+\b\s*)+ 
                |
                ""(.+?)""  
                )
                (?=\s*(?:\n|\(|\d{4}|-|:))
            ", RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

            // Từ khóa game phổ biến
            _gameKeywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "game", "dlc", "expansion", "edition", "remastered",
                "definitive edition", "season pass", "update", "patch"
            };
        }
        public async Task<string> ExtractGameTitleAsync(string content)
        {
            try
            {
                var matches = _titleRegex.Matches(content);

                foreach (Match match in matches)
                {
                    var candidate = match.Groups[1].Value.Trim('"', '\'');

                    if (await IsValidGameTitle(candidate))
                    {
                        return FormatGameTitle(candidate);
                    }
                }

                // Fallback: Tìm theo từ khóa
                foreach (Match match in matches)
                {
                    var candidate = match.Value;
                    if (ContainsGameKeywords(candidate))
                    {
                        return FormatGameTitle(candidate);
                    }
                }

                return "Latest Game News"; // Default value
            }
            catch
            {
                return "Game Update";
            }
        }
        private async Task<bool> IsValidGameTitle(string title)
        {
            try
            {
                // Kiểm tra qua IGDB API (https://api-docs.igdb.com)
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://api.igdb.com/v4/games"),
                    Headers =
                    {
                        { "Client-ID", "4syhacwzmcm3zll67javk0hsttxrh8" },
                        { "Authorization", "Bearer r3052bxjducaenl488e2smej5wyzwk" }
                    },
                    Content = new StringContent($"fields name; search \"{title}\"; limit 1;",
                        Encoding.UTF8, "application/text")
                };

                var response = await _httpClient.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();

                return !string.IsNullOrEmpty(result) &&
                       result.Contains(title, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                // Fallback: Kiểm tra local database
                return LocalGameDatabase.Contains(title);
            }
        }
        private bool ContainsGameKeywords(string text)
        {
            return text.Split(new[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries)
                .Any(word => _gameKeywords.Contains(word));
        }

        private string FormatGameTitle(string title)
        {
            // Chuẩn hóa định dạng tên game
            return Regex.Replace(title, @"\s+", " ")
                .Replace("�", "") // Loại bỏ ký tự lỗi
                .Trim();
        }
    }
}
