
namespace SimplePdfGenerator;

/// <summary>
/// Represents layout settings for a PDF page, such as columns, grids, or other layout options.
/// Provides helpers for unit conversion, coordinate conversion, and transformations.
/// </summary>
public class PageLayout
{
    /// <summary>
    /// Supported measurement units.
    /// </summary>
    public enum Unit
    {
        Points,
        Inches,
        Millimeters,
        Centimeters
    }

    /// <summary>
    /// Converts a value in the specified unit to points (PDF default: 72 points per inch).
    /// </summary>
    public static double ToPoints(double value, Unit unit)
    {
        return unit switch
        {
            Unit.Points => value,
            Unit.Inches => value * 72.0,
            Unit.Millimeters => value * 72.0 / 25.4,
            Unit.Centimeters => value * 72.0 / 2.54,
            _ => value
        };
    }

    /// <summary>
    /// Converts a value in points to the specified unit.
    /// </summary>
    public static double FromPoints(double points, Unit unit)
    {
        return unit switch
        {
            Unit.Points => points,
            Unit.Inches => points / 72.0,
            Unit.Millimeters => points * 25.4 / 72.0,
            Unit.Centimeters => points * 2.54 / 72.0,
            _ => points
        };
    }

    /// <summary>
    /// If true, treat (0,0) as the top-left corner for layout helpers.
    /// </summary>
    public bool UseTopLeftOrigin { get; set; } = false;

    /// <summary>
    /// Converts a point from top-left origin to PDF's bottom-left origin.
    /// </summary>
    public (double x, double y) ToPdfCoordinates(double x, double y, double pageHeight)
    {
        if (UseTopLeftOrigin)
        {
            return (x, pageHeight - y);
        }
        return (x, y);
    }

    /// <summary>
    /// Returns a transformation matrix for scaling.
    /// </summary>
    public static double[] GetScaleMatrix(double scaleX, double scaleY)
    {
        // [a, b, c, d, e, f] for scaling: [scaleX, 0, 0, scaleY, 0, 0]
        return new double[] { scaleX, 0, 0, scaleY, 0, 0 };
    }

    /// <summary>
    /// Returns a transformation matrix for rotation.
    /// </summary>
    public static double[] GetRotationMatrix(double angleDegrees)
    {
        double radians = angleDegrees * System.Math.PI / 180.0;
        double cos = System.Math.Cos(radians);
        double sin = System.Math.Sin(radians);
        // [cos, sin, -sin, cos, 0, 0]
        return new double[] { cos, sin, -sin, cos, 0, 0 };
    }

    /// <summary>
    /// Returns a transformation matrix for translation.
    /// </summary>
    public static double[] GetTranslationMatrix(double dx, double dy)
    {
        // [1, 0, 0, 1, dx, dy]
        return new double[] { 1, 0, 0, 1, dx, dy };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PageLayout"/> class.
    /// </summary>
    public PageLayout()
    {
        // Initialization logic for page layout
    }
}
