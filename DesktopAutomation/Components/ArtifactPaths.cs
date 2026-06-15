namespace DesktopAutomation.Components;

internal sealed class ArtifactPaths
{
    // Builds the desktop artifact folder set from one root path: pass, false, work, and log.
    private ArtifactPaths(string outputRoot)
    {
        OutputRoot = outputRoot;
        PassDir = Path.Combine(outputRoot, "pass");
        FalseDir = Path.Combine(outputRoot, "false");
        WorkDir = Path.Combine(outputRoot, "work");
        LogPath = Path.Combine(outputRoot, "desktop-run.log");
    }

    public string OutputRoot { get; }

    public string PassDir { get; }

    public string FalseDir { get; }

    public string WorkDir { get; }

    public string LogPath { get; }

    // Clears previous desktop outputs so a new run cannot accidentally reuse old screenshots/logs.
    public void CleanOutputRoot()
    {
        if (Directory.Exists(OutputRoot))
        {
            Directory.Delete(OutputRoot, recursive: true);
        }

        Directory.CreateDirectory(OutputRoot);
    }

    // Removes temporary files created while the app is running; pass/fail artifacts stay untouched.
    public void CleanWorkDir()
    {
        if (Directory.Exists(WorkDir))
        {
            Directory.Delete(WorkDir, recursive: true);
        }
    }

    // Reads TEST_RESULTS_DIR from Playwright, with a local fallback for direct dotnet runs.
    public static ArtifactPaths FromEnvironment()
    {
        var outputRoot = Environment.GetEnvironmentVariable("TEST_RESULTS_DIR")
            ?? Path.Combine(Directory.GetCurrentDirectory(), "test-results", "desktop");

        return new ArtifactPaths(outputRoot);
    }
}
