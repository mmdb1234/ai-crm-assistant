using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Features.AI_Assistans.Services;

namespace Infrastructure.AI_Assistans.Services;

public class TelegramBotService : ITelegramBotService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TelegramBotService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<TelegramBotInfo?> GetBotInfoAsync(string botToken)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetFromJsonAsync<TelegramBotInfo>(
            $"https://api.telegram.org/bot{botToken}/getMe");

        return response;
    }

    public async Task<bool> SetWebhookAsync(string botToken, string webhookUrl)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync(
            $"https://api.telegram.org/bot{botToken}/setWebhook",
            new { url = webhookUrl });

        return response.IsSuccessStatusCode;
    }
}
