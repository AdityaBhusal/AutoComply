using LiteDB;
using Microsoft.Extensions.Configuration;
using UglyToad.PdfPig;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

string apiKey = config["OpenAI:ApiKey"];
string model = config["OpenAI:Model"];


partial class Program
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
                System.Console.WriteLine("[Page: " + sentence.Page + "]");
                System.Console.WriteLine("[Category: " + sentence.Category + "]");
                System.Console.WriteLine("[Clause: " + sentence.Text + "]");
            }
        }
    }
}
