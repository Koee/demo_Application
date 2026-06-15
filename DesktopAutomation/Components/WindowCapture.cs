using System.Drawing;
using System.Drawing.Imaging;
using FlaUI.Core.AutomationElements;

namespace DesktopAutomation.Components;

internal static class WindowCapture
{
    // Captures the visible bounds of a desktop window into a PNG artifact.
    public static void Capture(Window window, string pngPath)
    {
        var bounds = window.BoundingRectangle;
        using var bitmap = new Bitmap((int)bounds.Width, (int)bounds.Height);

        using (var graphics = Graphics.FromImage(bitmap))
        {
            graphics.CopyFromScreen(
                (int)bounds.Left,
                (int)bounds.Top,
                0,
                0,
                bitmap.Size
            );
        }

        bitmap.Save(pngPath, ImageFormat.Png);
    }
}
