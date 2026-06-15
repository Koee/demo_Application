using DesktopAutomation.Runners;

internal static class Program
{
    // Thin entry point: delegate all setup, app flow, and pass/fail handling to the runner.
    [STAThread]
    private static int Main()
    {
        return DesktopTestRunner.Run();
    }
}
