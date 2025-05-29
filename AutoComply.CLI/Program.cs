using System.Data.Common;
using System.Security.Cryptography.X509Certificates;
using LiteDB;
using UglyToad.PdfPig;

class Program
{
    static async Task Main()
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
                var extractedClauses = await ClauseExtractorUtility.ClauseExtractor(
                    page.Text,
                    page.Number
                );
                foreach (var singleClause in extractedClauses.Take(1))
                {
                    if (
                        string.IsNullOrEmpty(singleClause.Text)
                        || clauses.Exists(x => x.Text == singleClause.Text)
                    )
                        continue;
                    clauses.Insert(singleClause);
                    insertionsCounter++;
                }
            }
            System.Console.WriteLine("Inserted clauses to db: " + insertionsCounter);

            foreach (var sentence in clauses.FindAll())
            {

                System.Console.WriteLine("[ID: " + sentence.Id + "]");
                System.Console.WriteLine("[Page: " + sentence.Page + "]");
                System.Console.WriteLine("[Category: " + sentence.Category + "]");
                System.Console.WriteLine("[Clause: " + sentence.Text + "]");
                System.Console.WriteLine("[Checklist: " + sentence.Checklist + "]");
            }
        }
    }
}
