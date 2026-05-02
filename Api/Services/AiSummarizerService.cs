using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Api.Services;

public class AiSummarizerService
{
    private readonly string? _apiKey;

    public AiSummarizerService(IConfiguration config)
    {
        _apiKey = config.GetValue<string>("GroqApiKey");
    }

    public async Task<string> ResumirNoticiaAsync(string textoOriginal, string categoria, string urlNoticia)
    {
        if (string.IsNullOrWhiteSpace(_apiKey))
        {
            return "Erro: Chave da API da Groq não configurada no appsettings.json.";
        }

        using var client = new HttpClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        string url = "https://api.groq.com/openai/v1/chat/completions";

        string instrucao = $@"
            Crie um resumo envolvente de 2 parágrafos para uma newsletter.
            Escreva EXCLUSIVAMENTE em Português do Brasil.
            
            Siga rigorosamente este formato:
            
            TÍTULO CHAMATIVO
            (Parágrafo 1: A notícia principal)
            (Parágrafo 2: O contexto ou a importância disso)

            Fonte: [Leia na íntegra no Motorsport Brasil]({urlNoticia})

            TEXTO ORIGINAL:
            {textoOriginal}
        ";

        var requestBody = new
        {
            model = "llama-3.3-70b-versatile",
            messages = new[]
            {
                new { role = "system", content = $"Você é um jornalista esportivo brasileiro de alta categoria, focado em {categoria}." },
                new { role = "user", content = instrucao }
            },
            temperature = 0.7
        };

        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        var response = await client.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(responseString);

            var resumo = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content").GetString();

            return resumo ?? "Resumo vazio gerado.";
        }

        var errorDetails = await response.Content.ReadAsStringAsync();
        return $"Erro ao chamar a IA ({response.StatusCode}): {errorDetails}";
    }
}