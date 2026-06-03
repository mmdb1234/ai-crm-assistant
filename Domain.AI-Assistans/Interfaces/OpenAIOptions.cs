

namespace Domain.AI_Assistans.Interfaces
{
    public class AIProvidersOptions
    {
        public string DefaultProvider { get; set; } = "OpenRouter";

        public AIProviderConfig OpenAI { get; set; } = new();

        public AIProviderConfig DeepSeek { get; set; } = new();

        public AIProviderConfig Gemini { get; set; } = new();

        public AIProviderConfig OpenRouter { get; set; } = new();
    }



    public class AIProviderConfig
    {
        public string ApiKey { get; set; } = "";
        public string Model { get; set; } = "";
        public string BaseUrl { get; set; } = "";
    }
}
