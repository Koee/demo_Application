namespace DesktopAutomation.Components;

internal static class AutomationLogger
{
    private static readonly object SyncRoot = new();
    private static string? logPath;

    // Initializes the desktop run log file before the runner starts writing steps and errors.
    public static void Configure(string filePath)
    {
        logPath = filePath;
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        File.WriteAllText(filePath, string.Empty);
    }

    // Writes a business-level automation step, for example open app, type, save, or capture.
    public static void Step(string message)
    {
        Write("STEP", message);
    }

    // Writes normal progress/debug information that helps trace a run after failure.
    public static void Info(string message)
    {
        Write("INFO", message);
    }

    // Writes failure details to both console output and the desktop log file.
    public static void Error(string message)
    {
        Write("ERROR", message);
    }

    // Single synchronized writer so parallel/async output cannot corrupt the log file.
    private static void Write(string level, string message)
    {
        var line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{level}] {message}";

        lock (SyncRoot)
        {
            Console.WriteLine(message);

            if (!string.IsNullOrWhiteSpace(logPath))
            {
                File.AppendAllText(logPath, line + Environment.NewLine);
            }
        }
    }
}
