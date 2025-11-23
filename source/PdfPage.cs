namespace SimplePdfGenerator;

/// <summary>
/// Represents a page size with width and height in points.
/// </summary>
public readonly struct PageSize
{
    public double Width { get; }
    public double Height { get; }

    public PageSize(double width, double height)
    {
        Width = width;
        Height = height;
    }
}

/// <summary>
/// Represents page margins in points.
/// </summary>
public readonly struct Margin
{
    public double Left { get; }
    public double Right { get; }
    public double Top { get; }
    public double Bottom { get; }

    public Margin(double left, double right, double top, double bottom)
    {
        Left = left;
        Right = right;
        Top = top;
        Bottom = bottom;
    }
}

/// <summary>
/// Represents a single page in a PDF document.
/// </summary>
public class PdfPage
{
    /// <summary>
    /// Adds text to the PDF content stream at the specified position, with font and size.
    /// </summary>
    /// <param name="text">The text to add.</param>
    /// <param name="x">The x-coordinate (user units).</param>
    /// <param name="y">The y-coordinate (user units).</param>
    /// <param name="fontName">The font resource name (e.g., F1).</param>
    /// <param name="fontSize">The font size in points.</param>
    /// <param name="layout">The PageLayout instance for coordinate conversion.</param>
    /// <param name="fontManager">The PdfFontManager instance for font lookup.</param>
    public void AddText(string text, double x, double y, string fontName, double fontSize, PageLayout layout, PdfFontManager fontManager)
    {
        // Convert coordinates if needed
        var (pdfX, pdfY) = layout.ToPdfCoordinates(x, y, Size.Height);
        // Use fontName as the PDF resource name (e.g., F1, F2, etc.)
        // Write PDF text operators: BT ... ET
        // Example: BT /F1 12 Tf 100 700 Td (Hello) Tj ET
        string fontResource = $"/{fontName}";
        string escapedText = text.Replace("(", "\\(").Replace(")", "\\)");
        string command = $"BT {fontResource} {fontSize} Tf {pdfX:0.##} {pdfY:0.##} Td ({escapedText}) Tj ET";
        AddCommand(command);
    }
    /// <summary>
    /// The content stream containing PDF drawing commands for this page as a byte stream.
    /// </summary>
    public System.IO.MemoryStream ContentStream { get; } = new System.IO.MemoryStream();

    /// <summary>
    /// Adds a drawing command to the content stream as bytes (UTF-8).
    /// </summary>
    /// <param name="command">The PDF operator command to add.</param>
    public void AddCommand(string command)
    {
        if (command == null) return;
        var bytes = System.Text.Encoding.UTF8.GetBytes(command + "\n");
        ContentStream.Write(bytes, 0, bytes.Length);
    }

    /// <summary>
    /// Clears all drawing commands from the content stream.
    /// </summary>
    public void ClearContentStream()
    {
        ContentStream.SetLength(0);
    }

    /// <summary>
    /// Gets the content stream as a byte array.
    /// </summary>
    public byte[] GetContentStreamBytes()
    {
        return ContentStream.ToArray();
    }

    /// <summary>
    /// Gets the content stream as a string (for debugging or text output).
    /// </summary>
    public string GetContentStreamAsString()
    {
        return System.Text.Encoding.UTF8.GetString(ContentStream.ToArray());
    }

    /// <summary>
    /// Page orientation options.
    /// </summary>
    public enum OrientationType
    {
        Portrait,
        Landscape
    }

    /// <summary>
    /// Common page size: A4 (595 x 842 points).
    /// </summary>
    public static readonly PageSize A4 = new PageSize(595, 842);

    /// <summary>
    /// Common page size: Letter (612 x 792 points).
    /// </summary>
    public static readonly PageSize Letter = new PageSize(612, 792);

    /// <summary>
    /// Common page size: Legal (612 x 1008 points).
    /// </summary>
    public static readonly PageSize Legal = new PageSize(612, 1008);

    /// <summary>
    /// Gets or sets the size of the page.
    /// </summary>
    public PageSize Size { get; private set; } = A4;

    /// <summary>
    /// Gets or sets the orientation of the page.
    /// </summary>
    public OrientationType Orientation { get; private set; } = OrientationType.Portrait;

    /// <summary>
    /// Gets or sets the page margins.
    /// </summary>
    public Margin Margins { get; private set; } = new Margin(40, 40, 40, 40); // Default 40pt margins

    /// <summary>
    /// Sets the size of the page.
    /// </summary>
    /// <param name="size">The page size to set.</param>
    public void SetPageSize(PageSize size)
    {
        Size = size;
    }

    /// <summary>
    /// Sets the orientation of the page.
    /// </summary>
    /// <param name="orientation">The orientation to set.</param>
    public void SetOrientation(OrientationType orientation)
    {
        Orientation = orientation;
    }

    /// <summary>
    /// Sets the page margins.
    /// </summary>
    /// <param name="margins">The margin struct to set.</param>
    public void SetMargins(Margin margins)
    {
        Margins = margins;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfPage"/> class.
    /// </summary>
    public PdfPage()

    {
        // Initialization logic for a PDF page
    }
}

