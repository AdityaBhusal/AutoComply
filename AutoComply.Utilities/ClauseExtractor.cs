using System.Text.RegularExpressions;

public static class ClauseExtractorUtility
{
    public static List<Clause> ClauseExtractor(string text, int page)
    {
        var clauses = new List<Clause>();

        if (string.IsNullOrEmpty(text))
            return clauses;

        var regSplitText = Regex.Split(text, @"?=\d\.\s");

        foreach (var splitText in regSplitPages)
        {
            var trimmedText = splitText.Trim();

            if (trimmedText.Text == null) continue;

            clauses.Add
            (
                new Clause
                {
                    Text = trimmedText,
                    Page = page,
                    Category = CategoryExtractor(trimmedText)
                }
            );
        }
        return clauses;
    }
}
