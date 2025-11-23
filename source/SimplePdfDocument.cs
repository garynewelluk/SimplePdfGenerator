
namespace SimplePdfGenerator;

/// <summary>
/// Provides simple PDF generation functionality.
/// </summary>
public class SimplePdfDocument
{
    /// <summary>
    /// Stores document-wide font resources.
    /// </summary>
    private readonly Dictionary<string, object> _fonts = new();

    /// <summary>
    /// Stores document-wide image resources.
    /// </summary>
    private readonly Dictionary<string, object> _images = new();

    /// <summary>
    /// Adds a font resource to the document.
    /// </summary>
    /// <param name="name">The unique name for the font.</param>
    /// <param name="fontResource">The font resource object.</param>
    public void AddFont(string name, object fontResource)
    {
        _fonts[name] = fontResource;
    }

    /// <summary>
    /// Gets a font resource by name.
    /// </summary>
    /// <param name="name">The unique name for the font.</param>
    /// <returns>The font resource object if found; otherwise, null.</returns>
    public object? GetFont(string name)
    {
        _fonts.TryGetValue(name, out var font);
        return font;
    }

    /// <summary>
    /// Adds an image resource to the document.
    /// </summary>
    /// <param name="name">The unique name for the image.</param>
    /// <param name="imageResource">The image resource object.</param>
    public void AddImage(string name, object imageResource)
    {
        _images[name] = imageResource;
    }

    /// <summary>
    /// Gets an image resource by name.
    /// </summary>
    /// <param name="name">The unique name for the image.</param>
    /// <returns>The image resource object if found; otherwise, null.</returns>
    public object? GetImage(string name)
    {
        _images.TryGetValue(name, out var image);
        return image;
    }
    /// <summary>
    /// Gets the collection of pages in the PDF document.
    /// </summary>
    public List<PdfPage> Pages { get; } = new List<PdfPage>();

    /// <summary>
    /// Adds a page to the PDF document.
    /// </summary>
    /// <param name="page">The page to add.</param>
    public void AddPage(PdfPage page)
    {
        Pages.Add(page);
    }

    /// <summary>
    /// Removes a page from the PDF document.
    /// </summary>
    /// <param name="page">The page to remove.</param>
    /// <returns>True if the page was removed; otherwise, false.</returns>
    public bool RemovePage(PdfPage page)
    {
        return Pages.Remove(page);
    }
    /// <summary>
    /// Gets or sets the title of the PDF document.
    /// </summary>
    public string? Title { get; private set; }

    /// <summary>
    /// Gets or sets the author of the PDF document.
    /// </summary>
    public string? Author { get; private set; }

    /// <summary>
    /// Gets or sets the subject of the PDF document.
    /// </summary>
    public string? Subject { get; private set; }

    /// <summary>
    /// Gets or sets the keywords for the PDF document.
    /// </summary>
    public string[]? Keywords { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SimplePdfDocument"/> class.
    /// </summary>
    public SimplePdfDocument()
    {
        // Initialization logic here
    }

    /// <summary>
    /// Sets the title of the PDF document.
    /// </summary>
    /// <param name="pdfTitle">The title to set.</param>
    public void SetTitle(string pdfTitle)
    {
        this.Title = pdfTitle;
    }

    /// <summary>
    /// Sets the author of the PDF document.
    /// </summary>
    /// <param name="pdfAuthor">The author to set.</param>
    public void SetAuthor(string pdfAuthor)
    {
        this.Author = pdfAuthor;
    }

    /// <summary>
    /// Sets the subject of the PDF document.
    /// </summary>
    /// <param name="pdfSubject">The subject to set.</param>
    public void SetSubject(string pdfSubject)
    {
        this.Subject = pdfSubject;
    }

    /// <summary>
    /// Sets the keywords for the PDF document.
    /// </summary>
    /// <param name="pdfKeywords">An array of keywords.</param>
    public void SetKeywords(string[] pdfKeywords)
    {
        this.Keywords = pdfKeywords;
    }

    /// <summary>
    /// Adds text to the PDF document.
    /// </summary>
    /// <param name="text">The text to add.</param>
    public void AddText(string text)
    {
        // Add text logic here
    }

    /// <summary>
    /// Saves the PDF document to the specified file.
    /// </summary>
    /// <param name="filePath">The file path to save the PDF.</param>
    public void Save(string filePath)
    {
        using var fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
        using var writer = new System.IO.StreamWriter(fs, System.Text.Encoding.ASCII);

        // PDF Header
        writer.WriteLine("%PDF-1.4");

        int objIndex = 1;
        var xref = new System.Collections.Generic.List<long>();
        xref.Add(0); // First entry is always 0

        // Font object (using Helvetica as example)
        long fontObjPos = fs.Position;
        xref.Add(fontObjPos);
        writer.WriteLine($"{objIndex} 0 obj");
        writer.WriteLine("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>");
        writer.WriteLine("endobj");
        int fontObjNum = objIndex++;

        // Write each page's content stream and page object
        var pageObjNums = new System.Collections.Generic.List<int>();
        var contentObjNums = new System.Collections.Generic.List<int>();
        foreach (var page in Pages)
        {
            // Content stream object
            long contentObjPos = fs.Position;
            xref.Add(contentObjPos);
            writer.WriteLine($"{objIndex} 0 obj");
            var contentBytes = page.GetContentStreamBytes();
            writer.WriteLine($"<< /Length {contentBytes.Length} >>");
            writer.WriteLine("stream");
            writer.Flush();
            fs.Write(contentBytes, 0, contentBytes.Length);
            writer.WriteLine();
            writer.WriteLine("endstream");
            writer.WriteLine("endobj");
            int contentObjNum = objIndex++;
            contentObjNums.Add(contentObjNum);

            // Page object
            long pageObjPos = fs.Position;
            xref.Add(pageObjPos);
            writer.WriteLine($"{objIndex} 0 obj");
            writer.WriteLine($"<< /Type /Page /Parent 0 0 R /MediaBox [0 0 {page.Size.Width} {page.Size.Height}] /Contents {contentObjNum} 0 R /Resources << /Font << /F1 {fontObjNum} 0 R >> >> >>");
            writer.WriteLine("endobj");
            pageObjNums.Add(objIndex++);
        }

        // Pages object
        long pagesObjPos = fs.Position;
        xref.Add(pagesObjPos);
        writer.WriteLine($"{objIndex} 0 obj");
        writer.WriteLine("<< /Type /Pages /Kids [");
        foreach (var pageNum in pageObjNums)
            writer.WriteLine($"{pageNum} 0 R");
        writer.WriteLine($"] /Count {Pages.Count} >>");
        writer.WriteLine("endobj");
        int pagesObjNum = objIndex++;

        // Catalog object
        long catalogObjPos = fs.Position;
        xref.Add(catalogObjPos);
        writer.WriteLine($"{objIndex} 0 obj");
        writer.WriteLine($"<< /Type /Catalog /Pages {pagesObjNum} 0 R >>");
        writer.WriteLine("endobj");
        int catalogObjNum = objIndex++;

        // XRef table
        long xrefPos = fs.Position;
        writer.WriteLine("xref");
        writer.WriteLine($"0 {xref.Count}");
        foreach (var pos in xref)
            writer.WriteLine($"{pos:0000000000} 00000 n ");
        // Trailer
        writer.WriteLine("trailer");
        writer.WriteLine($"<< /Size {xref.Count} /Root {catalogObjNum} 0 R >>");
        writer.WriteLine("startxref");
        writer.WriteLine(xrefPos);
        writer.WriteLine("%%EOF");
    }
}
