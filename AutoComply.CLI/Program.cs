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

            foreach (var page in pdfPages)
            {
                var extractedClauses = ClauseExtractorUtility.ClassExtractor(page);
                
            }
        }
    }
}
