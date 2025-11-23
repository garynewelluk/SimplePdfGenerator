namespace SimplePdfGenerator;

/// <summary>
/// Manages fonts for PDF generation, including registration and lookup.
/// </summary>
public class PdfFontManager
{
    // Common built-in PDF font names and their PDF resource names
    private static readonly Dictionary<string, string> BuiltInFonts = new()
    {
        { "Helvetica", "/Helv" },
        { "Helvetica-Bold", "/Helv-Bold" },
        { "Helvetica-Oblique", "/Helv-Oblique" },
        { "Helvetica-BoldOblique", "/Helv-BoldOblique" },
        { "Times-Roman", "/TiRo" },
        { "Times-Bold", "/TiBo" },
        { "Times-Italic", "/TiIt" },
        { "Times-BoldItalic", "/TiBI" },
        { "Courier", "/Cour" },
        { "Courier-Bold", "/Cour-Bold" },
        { "Courier-Oblique", "/Cour-Oblique" },
        { "Courier-BoldOblique", "/Cour-BoldOblique" },
        { "Symbol", "/Symb" },
        { "ZapfDingbats", "/ZaDb" }
    };

    private readonly Dictionary<string, string> _registeredFonts = new();

    /// <summary>
    /// Registers all common built-in PDF fonts for use by name.
    /// </summary>
    public void RegisterStandardFonts()
    {
        foreach (var kvp in BuiltInFonts)
        {
            if (!_registeredFonts.ContainsKey(kvp.Key))
                _registeredFonts[kvp.Key] = kvp.Value;
        }
    }

    /// <summary>
    /// Gets the PDF resource name for a built-in font, or null if not found.
    /// </summary>
    public string? GetBuiltInFontResource(string fontName)
    {
        return BuiltInFonts.TryGetValue(fontName, out var resource) ? resource : null;
    }

    /// <summary>
    /// Registers a font with a given name and file path.
    /// </summary>
    /// <param name="fontName">The name to reference the font by.</param>
    /// <param name="fontFilePath">The file path to the font file.</param>
    public void RegisterFont(string fontName, string fontFilePath)
    {
        if (!string.IsNullOrWhiteSpace(fontName) && !string.IsNullOrWhiteSpace(fontFilePath))
        {
            _registeredFonts[fontName] = fontFilePath;
        }
    }

    /// <summary>
    /// Gets the file path for a registered font by name.
    /// </summary>
    /// <param name="fontName">The name of the font.</param>
    /// <returns>The file path if found, otherwise null.</returns>
    public string? GetFontPath(string fontName)
    {
        return _registeredFonts.TryGetValue(fontName, out var path) ? path : null;
    }

    /// <summary>
    /// Lists all registered font names.
    /// </summary>
    public IEnumerable<string> GetRegisteredFonts() => _registeredFonts.Keys;
}
