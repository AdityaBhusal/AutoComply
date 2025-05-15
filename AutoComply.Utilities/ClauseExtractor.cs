using System.Text.RegularExpressions;
using CategoryExtractor;

public static class ClauseExtractorUtility
{
    public static List<Clause> ClauseExtractor(string text, int page)
    {
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
                    Category = CategoryExtractorUtility.CategoryExtractor(trimmedText)
                }
            );
        }
        return clauses;
    }
}
