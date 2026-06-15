using DesktopAutomation.Apps;
using DesktopAutomation.Components;

namespace DesktopAutomation.Runners;

internal static class DesktopTestRunner
{
    // Main desktop lifecycle: prepare artifacts/config, run the app flow, and convert failures into debuggable outputs.
    public static int Run()
    {
        var artifacts = ArtifactPaths.FromEnvironment();
        var config = TestRunConfig.FromEnvironment();
        var notepad = new NotepadAutomation();

        try
        {
            // Start each run from a clean desktop output folder so old artifacts cannot hide the real result.
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

            // Remove stale app instances before starting the scenario to avoid focusing the wrong window.
            ProcessManager.KillProcessesByName("notepad");

            notepad.Run(artifacts, config);

            AutomationLogger.Info("RESULT: PASS");
            return 0;
        }
        catch (Exception ex)
        {
            // Failure path: always save the exception, then try to capture the current desktop state.
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
            // Cleanup should run for both pass and fail, but must not replace the real test outcome.
            ProcessManager.KillProcessesByName("notepad");
            TryCleanWorkDir(artifacts);
        }
    }

    // Best-effort failure screenshot for debugging UI state without masking the original exception.
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

    // Best-effort cleanup for temporary work files created during the desktop flow.
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
