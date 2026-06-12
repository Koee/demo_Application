using DesktopAutomation.Components;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using FlaUI.UIA3;
using FlaUIApplication = FlaUI.Core.Application;

namespace DesktopAutomation.Apps;

internal sealed class NotepadAutomation
{
    public Window? CurrentWindow { get; private set; }

    public void Run(ArtifactPaths artifacts, TestRunConfig config)
    {
        var text = TestDataReader.ReadNotepadText();
        Directory.CreateDirectory(artifacts.WorkDir);

        var txtPath = Path.Combine(artifacts.WorkDir, $"{config.ArtifactPrefix}-output.txt");
        File.WriteAllText(txtPath, string.Empty);

        using var automation = new UIA3Automation();

        AutomationLogger.Step("STEP 1: Open Notepad");
        var notepad = FlaUIApplication.Launch("notepad.exe", $"\"{txtPath}\"");
        CurrentWindow = WindowFinder.WaitForMainWindow(notepad, automation, "Notepad");

        WindowActivator.BringToFront(CurrentWindow);

        AutomationLogger.Step("STEP 2: Type content into Notepad");
        EnterAndVerifyText(txtPath, text, config);

        AutomationLogger.Step("STEP 4: Capture Notepad screenshot");
        WindowActivator.BringToFront(CurrentWindow);
        Thread.Sleep(500);

        Directory.CreateDirectory(artifacts.PassDir);
        var outputPath = Path.Combine(artifacts.PassDir, $"{config.ArtifactPrefix}-output.txt");
        var pngPath = Path.Combine(artifacts.PassDir, $"{config.ArtifactPrefix}-screenshot.png");
        File.Copy(txtPath, outputPath, overwrite: true);
        WindowCapture.Capture(CurrentWindow, pngPath);

        AutomationLogger.Info($"Saved screenshot: {pngPath}");
        AutomationLogger.Info($"Saved text file: {outputPath}");
    }

    private void EnterAndVerifyText(string txtPath, string expectedText, TestRunConfig config)
    {
        for (var attempt = 1; attempt <= config.TextEntryMaxAttempts; attempt++)
        {
            if (CurrentWindow == null)
            {
                throw new InvalidOperationException("Notepad window is not available.");
            }

            AutomationLogger.Step($"STEP 2.{attempt}: Focus editor and paste text");
            TextInput.ReplaceTextInEditor(CurrentWindow, expectedText);
            Thread.Sleep(500);

            AutomationLogger.Step($"STEP 3.{attempt}: Save Notepad file");
            SaveFile();
            Thread.Sleep(1000);

            var savedText = ReadSavedText(txtPath);

            if (string.Equals(savedText, expectedText, StringComparison.Ordinal))
            {
                AutomationLogger.Info("Verified saved text matches input data.");
                return;
            }

            AutomationLogger.Info($"Saved text mismatch on attempt {attempt}. Actual: '{savedText}'");
        }

        throw new InvalidOperationException(
            $"Notepad did not save expected text after {config.TextEntryMaxAttempts} attempts. Expected '{expectedText}', actual '{ReadSavedText(txtPath)}'.");
    }

    private static void SaveFile()
    {
        Keyboard.Press(VirtualKeyShort.CONTROL);
        Keyboard.Press(VirtualKeyShort.KEY_S);
        Keyboard.Release(VirtualKeyShort.KEY_S);
        Keyboard.Release(VirtualKeyShort.CONTROL);
    }

    private static string ReadSavedText(string txtPath)
    {
        return File.Exists(txtPath)
            ? File.ReadAllText(txtPath).Trim()
            : string.Empty;
    }
}
