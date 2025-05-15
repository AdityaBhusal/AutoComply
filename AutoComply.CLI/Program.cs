using LiteDB;
using UglyToad.PdfPig;

class Program
{
    static void Main()
    {
        var pdfPath = "C:/Users/Asus/OneDrive/Desktop/samplepdff.pdf";
        PdfDocument pdfDocument = PdfDocument.Open(pdfPath);

        var pdfPages = pdfDocument.GetPages();

        using (
            var db = new LiteDatabase(
                @"C:/Users/Asus/OneDrive/Desktop/Coding Projects/AutoComply/AutoComply.Data/MyData.db"
            )
        )
        {
            var clauses = db.GetCollection<Clause>("clauses");
            var insertionsCounter = 0;

            foreach (var page in pdfPages)
            {
                var extractedClauses = ClauseExtractorUtility.ClauseExtractor(
                    page.Text,
                    page.Number
                );
                foreach (var singleClause in extractedClauses)
                {
                    if (string.IsNullOrEmpty(singleClause.Text))
                        continue;
                    clauses.Insert(singleClause);
                    insertionsCounter++;
                }
            }
            System.Console.WriteLine("Total insertions made to db: " + insertionsCounter);
        }
    }
}
