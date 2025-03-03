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
}
