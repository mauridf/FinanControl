using Microsoft.Maui.Graphics;

namespace FinanControl.App.Extensions;

public static class ColorExtensions
{
    public static string ToHex(this Color color)
    {
        if (color == null)
            return "#000000";

        var red = (int)(color.Red * 255);
        var green = (int)(color.Green * 255);
        var blue = (int)(color.Blue * 255);
        var alpha = (int)(color.Alpha * 255);

        if (alpha < 255)
            return $"#{alpha:X2}{red:X2}{green:X2}{blue:X2}";
        else
            return $"#{red:X2}{green:X2}{blue:X2}";
    }
}