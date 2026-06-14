using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AIChatBotAppRAG.Models;

namespace AIChatBotAppRAG.Services
{
    public class ChatBotService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public ChatBotService(string apiUrl)
        {
            _apiUrl = apiUrl;
            _httpClient = new HttpClient();
        }

        public async Task<Tuple<string, string>> SendMessageAsync(ChatRequest chatRequest)
        {
            try
            {
                var json = JsonSerializer.Serialize(chatRequest);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_apiUrl, content);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseJson);
                if (doc.RootElement.TryGetProperty("aiResponse", out var responseProp))
                {
                    var aiResponse = responseProp.GetString();
                    var source = doc.RootElement.TryGetProperty("mode", out var sourceProp) ? sourceProp.GetString() : string.Empty;
                    return Tuple.Create(aiResponse, source);
                }

    
                return Tuple.Create("Sorry, I couldn't process that.", string.Empty);
            }
            catch (Exception ex)
            {
                return Tuple.Create($"Error: {ex.Message}", string.Empty);
            }
        }

        public async Task<string> SendChatRequestAsync(ChatRequest chatRequest)
        {
            try
            {
                var json = JsonSerializer.Serialize(chatRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_apiUrl, content);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}