using System.Runtime.InteropServices;
using FlaUI.Core.AutomationElements;

namespace DesktopAutomation.Components;

internal static class WindowActivator
{
    private const int Restore = 9;
    private const int Show = 5;

    public static void BringToFront(Window window)
    {
        var handle = new IntPtr(window.Properties.NativeWindowHandle.Value);

        ShowWindow(handle, Restore);
        ShowWindow(handle, Show);
        SetForegroundWindow(handle);
        window.Focus();
        Thread.Sleep(500);
    }

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);
}
