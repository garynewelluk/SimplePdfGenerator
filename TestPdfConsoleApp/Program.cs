using SimplePdfGenerator;
using System;

class Program
{
    static void Main()
    {
        var doc = new SimplePdfDocument();
        var page = new PdfPage();
        var layout = new PageLayout();
        var fontManager = new PdfFontManager();
        fontManager.RegisterStandardFonts();

        // Add text to the page
        page.AddText("Hello, PDF!", 100, 700, "Helvetica", 12, layout, fontManager);

        doc.AddPage(page);
        doc.Save("test.pdf");

        // Simple check: file exists and is not empty
        if (System.IO.File.Exists("test.pdf") && new System.IO.FileInfo("test.pdf").Length > 0)
            Console.WriteLine("PDF created successfully!");
        else
            Console.WriteLine("PDF creation failed.");
    }
}
