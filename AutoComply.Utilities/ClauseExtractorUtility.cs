using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CategoryExtractor;
using Microsoft.Extensions.Configuration;

// Modified ClauseExtractorUtility.cs
public static class ClauseExtractorUtility
{
    public static async Task<List<Clause>> ClauseExtractor(string text, int page)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        var generator = new QwenChecklistGenerator(config["HuggingFace:ApiKey"]);



        var clauses = new List<Clause>();

        if (string.IsNullOrEmpty(text))
            return clauses;

        var regSplitText = Regex.Split(text, @"(?=\n?\s*\d+\.\s)");

        foreach (var splitText in regSplitText)
        {
            var trimmedText = splitText.Trim();

            if (trimmedText == null)
                continue;

            clauses.Add(
                new Clause
                {
                    Text = trimmedText,
                    Page = page,
                    Category = CategoryExtractorUtility.CategoryExtractor(trimmedText),
                    Checklist = await generator.GenerateChecklistAsync(trimmedText)
                }
            );
        }
        return clauses;
    }
}
