namespace DesktopAutomation.Components;

internal static class AutomationLogger
{
    private static readonly object SyncRoot = new();
    private static string? logPath;

    public static void Configure(string filePath)
    {
        logPath = filePath;
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        File.WriteAllText(filePath, string.Empty);
    }

    public static void Step(string message)
    {
        Write("STEP", message);
    }

    public static void Info(string message)
    {
        Write("INFO", message);
    }

    public static void Error(string message)
    {
        Write("ERROR", message);
    }

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
