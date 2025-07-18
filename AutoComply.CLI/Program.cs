using System.Linq;
using AutoComply.Utilities;
using CategoryExtractor;
using LiteDB;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

class Program
{
    static void Main()
    {
        using (
            PdfDocument document = PdfDocument.Open("C:/Users/Asus/OneDrive/Desktop/samplepdff.pdf")
        )
        using (var db = new LiteDatabase(@"MyData.db"))
        {
            var ClauseList = db.GetCollection<Clause>("Clauses");
            foreach (Page page in document.GetPages())
            {
                System.Console.WriteLine("here");
                if (ClauseExtractorUtility.Trimmer(page.Text) is null)
                    continue;

                foreach (var stringClause in ClauseExtractorUtility.Trimmer(page.Text))
                {
                    ClauseList.Insert(new Clause()
                    {
                        Text = stringClause,
                        Category = CategoryExtractorUtility.CategoryExtractor(stringClause)
                     });
                }
            }
            foreach (var clause in ClauseList.FindAll())
            {
                System.Console.WriteLine("ClauseId: " + clause.Id);
                System.Console.WriteLine("ClauseCategory: " + clause.Category);
                System.Console.WriteLine("ClauseText: " + clause.Text);
            }
        }
    }
}
