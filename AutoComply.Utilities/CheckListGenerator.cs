// Modified ChecklistGenerator.cs
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class QwenChecklistGenerator
{
    private readonly HttpClient _client = new();
    private const string ApiUrl =
        "https://api-inference.huggingface.co/models/Qwen/Qwen3-235B-A22B";

    public QwenChecklistGenerator(string hfApiKey)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            hfApiKey
        );
    }

    public async Task<string> GenerateChecklistAsync(string clauseText)
    {
        var prompt =
            $@"Convert this legal clause into a numbered checklist. Use 5 items maximum.
        Clause: {clauseText}
        Checklist: 1.";

        var response = await _client.PostAsync(
            ApiUrl,
            new StringContent(
                JsonSerializer.Serialize(
                    new
                    {
                        inputs = prompt,
                        parameters = new { max_new_tokens = 150, temperature = 0.3 }
                    }
                ),
                Encoding.UTF8,
                "application/json"
            )
        );

        var content = await response.Content.ReadAsStringAsync();
        return ParseChecklistResponse(content);
    }

    private string? ParseChecklistResponse(string jsonResponse)
    {
        using var doc = JsonDocument.Parse(jsonResponse);
        return doc.RootElement[0]
            .GetProperty("generated_text")
            .GetString()
            ?.Split("Checklist:")[1]
            .Trim();
    }
}
