using System.Diagnostics;

namespace DesktopAutomation.Components;

internal static class ProcessManager
{
    // Kills stale app processes before/after a run so desktop focus and window lookup stay deterministic.
    public static void KillProcessesByName(string processName)
    {
        foreach (var process in Process.GetProcessesByName(processName))
        {
            try
            {
                process.Kill();
                process.WaitForExit(2000);
            }
            catch
            {
                // Cleanup should not hide the real test result.
            }
        }
    }
}
