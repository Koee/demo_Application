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

    // App flow: open Notepad with a temp file, enter test data, verify the saved file, then publish pass artifacts.
    public void Run(ArtifactPaths artifacts, TestRunConfig config)
    {
        var text = TestDataReader.ReadNotepadText();
        Directory.CreateDirectory(artifacts.WorkDir);

        var txtPath = Path.Combine(artifacts.WorkDir, $"{config.ArtifactPrefix}-output.txt");
        File.WriteAllText(txtPath, string.Empty);

        // UIA3Automation is the FlaUI backend used to find and interact with Windows UI elements.
        using var automation = new UIA3Automation();

        AutomationLogger.Step("STEP 1: Open Notepad");
        var notepad = FlaUIApplication.Launch("notepad.exe", $"\"{txtPath}\"");
        CurrentWindow = WindowFinder.WaitForMainWindow(notepad, automation, "Notepad");

        WindowActivator.BringToFront(CurrentWindow);

        AutomationLogger.Step("STEP 2: Type content into Notepad");
        EnterAndVerifyText(txtPath, text, config);

        AutomationLogger.Step("STEP 4: Capture Notepad screenshot");
        // Pass artifacts are created only after the saved file has been verified.
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

    // Retry loop for the flaky part of desktop UI: focus editor, paste text, save, then verify the file content.
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

    // Sends Ctrl+S to persist the current Notepad buffer to the file opened at launch.
    private static void SaveFile()
    {
        Keyboard.Press(VirtualKeyShort.CONTROL);
        Keyboard.Press(VirtualKeyShort.KEY_S);
        Keyboard.Release(VirtualKeyShort.KEY_S);
        Keyboard.Release(VirtualKeyShort.CONTROL);
    }

    // Reads the saved temp file as the source of truth for pass/fail verification.
    private static string ReadSavedText(string txtPath)
    {
        return File.Exists(txtPath)
            ? File.ReadAllText(txtPath).Trim()
            : string.Empty;
    }
}
