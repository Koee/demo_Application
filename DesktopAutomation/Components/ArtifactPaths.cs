namespace DesktopAutomation.Components;

internal sealed class ArtifactPaths
{
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

    public void CleanOutputRoot()
    {
        if (Directory.Exists(OutputRoot))
        {
            Directory.Delete(OutputRoot, recursive: true);
        }

        Directory.CreateDirectory(OutputRoot);
    }

    public void CleanWorkDir()
    {
        if (Directory.Exists(WorkDir))
        {
            Directory.Delete(WorkDir, recursive: true);
        }
    }

    public static ArtifactPaths FromEnvironment()
    {
        var outputRoot = Environment.GetEnvironmentVariable("TEST_RESULTS_DIR")
            ?? Path.Combine(Directory.GetCurrentDirectory(), "test-results", "desktop");

        return new ArtifactPaths(outputRoot);
    }
}
