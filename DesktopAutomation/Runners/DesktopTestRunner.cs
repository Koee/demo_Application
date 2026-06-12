using DesktopAutomation.Apps;
using DesktopAutomation.Components;

namespace DesktopAutomation.Runners;

internal static class DesktopTestRunner
{
    public static int Run()
    {
        var artifacts = ArtifactPaths.FromEnvironment();
        var config = TestRunConfig.FromEnvironment();
        var notepad = new NotepadAutomation();

        try
        {
            artifacts.CleanOutputRoot();
            AutomationLogger.Configure(artifacts.LogPath);
            AutomationLogger.Info($"Artifact root: {artifacts.OutputRoot}");
            AutomationLogger.Info($"Environment: {config.EnvironmentName}");
            AutomationLogger.Info($"Scenario: {config.ScenarioName}");

            if (!OperatingSystem.IsWindows())
            {
                AutomationLogger.Info("RESULT: FAIL");
                AutomationLogger.Error("Desktop automation requires Windows.");
                return 1;
            }

            ProcessManager.KillProcessesByName("notepad");

            notepad.Run(artifacts, config);

            AutomationLogger.Info("RESULT: PASS");
            return 0;
        }
        catch (Exception ex)
        {
            Directory.CreateDirectory(artifacts.FalseDir);
            File.WriteAllText(Path.Combine(artifacts.FalseDir, "error.txt"), ex.ToString());

            if (notepad.CurrentWindow != null)
            {
                TryCaptureFailureScreenshot(notepad.CurrentWindow, artifacts);
            }

            AutomationLogger.Info("RESULT: FAIL");
            AutomationLogger.Error(ex.ToString());
            return 1;
        }
        finally
        {
            ProcessManager.KillProcessesByName("notepad");
            TryCleanWorkDir(artifacts);
        }
    }

    private static void TryCaptureFailureScreenshot(FlaUI.Core.AutomationElements.Window window, ArtifactPaths artifacts)
    {
        try
        {
            WindowCapture.Capture(window, Path.Combine(artifacts.FalseDir, "notepad-screenshot.png"));
        }
        catch
        {
            // Preserve the original failure details.
        }
    }

    private static void TryCleanWorkDir(ArtifactPaths artifacts)
    {
        try
        {
            artifacts.CleanWorkDir();
        }
        catch
        {
            // Cleanup should not hide the real test result.
        }
    }
}
