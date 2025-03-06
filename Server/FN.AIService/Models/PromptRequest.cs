namespace GeminiAIDev.Models
{
    public class PromptRequest
    {
        public string Prompt { get; set; }
    }
    public class PromptDescriptionRequest
    {
        public string Name { get; set; }
        public string System { get; set; }
        public string Other { get; set; }
    }
    public class PostResponse
    {
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public string Description { get; set; }
        public string DetailHtml { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
