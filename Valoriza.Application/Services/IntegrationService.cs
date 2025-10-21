using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;


namespace Valoriza.Application.Services;


public class IntegrationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;


    public IntegrationService(IHttpClientFactory httpClientFactory, IConfiguration config)
    { _httpClientFactory = httpClientFactory; _config = config; }


    public async Task<object?> CepLookupAsync(string cep)
    {
        var client = _httpClientFactory.CreateClient("brasilapi");
        return await client.GetFromJsonAsync<object>($"cep/v1/{cep}");
    }


    public async Task<string> ExplainSpendingWithOpenAIAsync(string text)
    {
        var apiKey = _config["OpenAI:ApiKey"] ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
            return "OpenAI API key not configured.";


        var client = _httpClientFactory.CreateClient("openai");
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);


        var payload = new
        {
            model = "gpt-4o-mini",
            messages = new[] { new { role = "user", content = $"Explique em linguagem simples: {text}" } }
        };


        using var res = await client.PostAsJsonAsync("chat/completions", payload);
        res.EnsureSuccessStatusCode();
        var json = await res.Content.ReadFromJsonAsync<dynamic>();
        return (string?)json?.choices?[0]?.message?.content ?? "Sem resposta";
    }
}