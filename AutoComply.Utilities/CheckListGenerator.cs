using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

public class ChecklistGenerator
{
    private readonly Kernel _kernel;

    public ChecklistGenerator(string apiKey)
    {
        _kernel = Kernel.CreateBuilder().AddOpenAIChatCompletion("gpt-3.5-turbo", apiKey).Build();
    }

    public async Task<string> GenerateChecklistAsync(string clauseText)
    {
        var prompt =
            @"
Instruction: Convert the legal clause below into a simple checklist item.
Clause: {{$input}}
Checklist:";

        var func = _kernel.CreateFunctionFromPrompt(
            prompt,
            new OpenAIPromptExecutionSettings { MaxTokens = 150 }
        );
        var result = await _kernel.InvokeAsync(func, new() { ["input"] = clauseText });
        return result.ToString().Trim();
    }
}
