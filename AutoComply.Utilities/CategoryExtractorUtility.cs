namespace CategoryExtractor
{
    public static class CategoryExtractorUtility
    {
        public static string CategoryExtractor(string text)
        {
            var lower = text.ToLowerInvariant();

            if (
                lower.Contains("gdpr")
                || lower.Contains("personal data")
                || lower.Contains("privacy")
            )
                return "Privacy";

            if (lower.Contains("security") || lower.Contains("access control"))
                return "Security";

            if (lower.Contains("record") || lower.Contains("retention") || lower.Contains("audit"))
                return "Record-Keeping";

            if (
                lower.Contains("bribe")
                || lower.Contains("fraud")
                || lower.Contains("whistleblower")
            )
                return "Anti-Corruption";

            if (lower.Contains("training") || lower.Contains("awareness"))
                return "Training & Awareness";

            return null;
        }
    }
}
