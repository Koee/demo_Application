using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using FlaUI.UIA3;
using FlaUIApplication = FlaUI.Core.Application;

class Program
{
    [STAThread]
    static int Main()
    {
        var outputRoot = Environment.GetEnvironmentVariable("TEST_RESULTS_DIR")
            ?? Path.Combine(Directory.GetCurrentDirectory(), "test-results");
        var passDir = Path.Combine(outputRoot, "pass");
        var falseDir = Path.Combine(outputRoot, "false");
        Window? notepadWindow = null;

        try
        {
            if (!OperatingSystem.IsWindows())
            {
                Console.WriteLine("RESULT: FAIL");
                Console.WriteLine("Desktop automation requires Windows.");
                return 1;
            }

            var text = ReadNotepadText();
            var workingDir = Path.Combine(Path.GetTempPath(), "desktop-at-output");

            Directory.CreateDirectory(workingDir);

            var txtPath = Path.Combine(workingDir, "notepad-output.txt");
            File.WriteAllText(txtPath, string.Empty);

            KillProcess("notepad");

            using var automation = new UIA3Automation();

            Console.WriteLine("STEP 1: Open Notepad");
            var notepad = FlaUIApplication.Launch("notepad.exe", $"\"{txtPath}\"");
            notepadWindow = WaitForMainWindow(notepad, automation, "Notepad");

            notepadWindow.Focus();

            Console.WriteLine("STEP 2: Type content into Notepad");
            Keyboard.Type(text);
            Thread.Sleep(500);

            Console.WriteLine("STEP 3: Save Notepad file");
            Keyboard.Press(VirtualKeyShort.CONTROL);
            Keyboard.Press(VirtualKeyShort.KEY_S);
            Keyboard.Release(VirtualKeyShort.KEY_S);
            Keyboard.Release(VirtualKeyShort.CONTROL);

            Thread.Sleep(1000);

            Console.WriteLine("STEP 4: Capture Notepad screenshot");
            notepadWindow.Focus();
            Thread.Sleep(500);

            Directory.CreateDirectory(passDir);
            var pngPath = Path.Combine(passDir, "notepad-screenshot.png");
            CaptureWindow(notepadWindow, pngPath);

            Console.WriteLine($"Saved screenshot: {pngPath}");
            Console.WriteLine($"Saved text file: {txtPath}");
            Console.WriteLine("RESULT: PASS");
            return 0;
        }
        catch (Exception ex)
        {
            Directory.CreateDirectory(falseDir);
            File.WriteAllText(Path.Combine(falseDir, "error.txt"), ex.ToString());

            if (notepadWindow != null)
            {
                try
                {
                    CaptureWindow(notepadWindow, Path.Combine(falseDir, "notepad-screenshot.png"));
                }
                catch
                {
                    // Ignore screenshot errors while handling the original failure.
                }
            }

            Console.WriteLine("RESULT: FAIL");
            Console.WriteLine(ex);
            return 1;
        }
        finally
        {
            KillProcess("notepad");
        }
    }

    static Window WaitForMainWindow(FlaUIApplication app, UIA3Automation automation, string expectedName, int timeoutMs = 10000)
    {
        var start = DateTime.Now;

        while ((DateTime.Now - start).TotalMilliseconds < timeoutMs)
        {
            var window = app.GetMainWindow(automation);

            if (window != null)
            {
                return window;
            }

            Thread.Sleep(300);
        }

        throw new Exception($"Cannot find main window: {expectedName}");
    }

    static void CaptureWindow(Window window, string pngPath)
    {
        var bounds = window.BoundingRectangle;
        using var bitmap = new Bitmap((int)bounds.Width, (int)bounds.Height);

        using (var graphics = Graphics.FromImage(bitmap))
        {
            graphics.CopyFromScreen(
                (int)bounds.Left,
                (int)bounds.Top,
                0,
                0,
                bitmap.Size
            );
        }

        bitmap.Save(pngPath, ImageFormat.Png);
    }

    static string ReadNotepadText()
    {
        var textFilePath = Environment.GetEnvironmentVariable("NOTEPAD_TEXT_FILE")
            ?? Path.Combine(AppContext.BaseDirectory, "TestData", "notepad-text.txt");

        if (!File.Exists(textFilePath))
        {
            return "Automation Test Notepad";
        }

        var text = File.ReadAllText(textFilePath).Trim();

        return string.IsNullOrWhiteSpace(text)
            ? "Automation Test Notepad"
            : text;
    }

    static void KillProcess(string processName)
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
                // Ignore cleanup errors.
            }
        }
    }
}
