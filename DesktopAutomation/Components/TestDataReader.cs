namespace DesktopAutomation.Components;

internal static class TestDataReader
{
    private const string DefaultNotepadText = "Automation Test Notepad";
    private static readonly string[] DefaultNotepadDataPath = ["test-data", "desktop", "notepad", "notepad-text.txt"];

    // Loads Notepad input text from env/file and falls back to a safe default for direct local runs.
    public static string ReadNotepadText()
    {
        var textFilePath = Environment.GetEnvironmentVariable("NOTEPAD_TEXT_FILE")
            ?? FindFromCurrentLocations(DefaultNotepadDataPath);

        if (string.IsNullOrWhiteSpace(textFilePath) || !File.Exists(textFilePath))
        {
            return DefaultNotepadText;
        }

        var text = File.ReadAllText(textFilePath).Trim();

        return string.IsNullOrWhiteSpace(text)
            ? DefaultNotepadText
            : text;
    }

    // Finds test data by walking upward from common runtime folders such as repo root or build output.
    private static string? FindFromCurrentLocations(IReadOnlyList<string> relativeParts)
    {
        foreach (var startPath in new[] { Directory.GetCurrentDirectory(), AppContext.BaseDirectory })
        {
            var directory = new DirectoryInfo(startPath);

            while (directory != null)
            {
                var candidate = Path.Combine(new[] { directory.FullName }.Concat(relativeParts).ToArray());

                if (File.Exists(candidate))
                {
                    return candidate;
                }

                directory = directory.Parent;
            }
        }

        return null;
    }
}
