using System.Text.Json.Serialization;

namespace AIChatBotAppRAG.Models
{
    public class ChatRequest
    {
        [JsonPropertyName("UserInput")]
        public string UserInput { get; set; } = string.Empty;

        [JsonPropertyName("Model")]
        public string Model { get; set; } = string.Empty;

        [JsonPropertyName("Provider")]
        public string Provider { get; set; } = string.Empty;

        [JsonPropertyName("input_type")]
        public string InputType { get; set; } = string.Empty;

        [JsonPropertyName("UseRag")]
        public bool UseRag { get; set; }
    }
}
