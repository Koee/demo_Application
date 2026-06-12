using DesktopAutomation.Runners;

internal static class Program
{
    [STAThread]
    private static int Main()
    {
        return DesktopTestRunner.Run();
    }
}
