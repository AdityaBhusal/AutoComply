using System.Text.RegularExpressions;
using Azure;
using Azure.AI.Inference;
using CategoryExtractor;
using Microsoft.Extensions.Configuration;

namespace AutoComply.Utilities
{
    public static partial class ClauseExtractorUtility
    {
        public static List<string> Trimmer(string chunkString)
        {
            var finalClause = new List<string>();

            var endpoint = new Uri("https://models.github.ai/inference");
            var credential = new AzureKeyCredential("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");
            var model = "openai/gpt-4o";

            var client = new ChatCompletionsClient(
                endpoint,
                credential,
                new AzureAIInferenceClientOptions()
            );

            // var id = 0;
            foreach (var sentence in MyRegex().Split(chunkString))
            {
                System.Console.WriteLine("-----------------------starting trim------------------");
                var trimmed = sentence.Trim();
                if (string.IsNullOrEmpty(trimmed))
                    continue;

                // Example: Add with a default key (e.g., false) and the trimmed string as value
                // Add the trimmed string with a placeholder key (e.g., default or null for now)

                // Prepare the request for the model
                var options = new ChatCompletionsOptions()
                {
                    Messages =
                    {
                        new ChatRequestSystemMessage(
                            "You're an expert in legal and compliance language."
                        ),
                        new ChatRequestUserMessage(
                            $"Is the following sentence a valid compliance clause? Respond only with 0 or 1 (for false and true): {trimmed}"
                        ),
                    },
                    Model = model,
                    Temperature = 1f,
                    // NucleusSamplingFactor = 1f,
                };

                // Send the request synchronously (to avoid too many concurrent requests)
                var response = client.Complete(options);
                Console.WriteLine(
                    "-----------------------response from model------------------------"
                );
                // Extract the content from the response
                var responseContent = response.Value.Content.Trim();
                // System.Console.WriteLine(responseContent);
                int temp = int.Parse(responseContent);
                bool isValidClause;
                isValidClause = Convert.ToBoolean(temp);
                // System.Console.WriteLine(isValidClause);
                // var responseContent = response.Value.Choices[0].Message.Content.Trim();

                if (!isValidClause)
                    continue;

                //extract category of the clause
                // id++;
                finalClause.Add(trimmed);
                System.Console.WriteLine("-----------clause added-----------");
            }

            return finalClause;
        }
        [System.Text.RegularExpressions.GeneratedRegex(@"(?<=[.!?])\s+")]
        private static partial System.Text.RegularExpressions.Regex MyRegex();
    }
}
