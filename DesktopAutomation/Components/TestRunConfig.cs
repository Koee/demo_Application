using System.Text.RegularExpressions;

namespace DesktopAutomation.Components;

internal sealed class TestRunConfig
{
    private const string DefaultScenarioName = "notepad-save-text";
    private const string DefaultEnvironmentName = "local";
    private const int DefaultTextEntryMaxAttempts = 3;

    private TestRunConfig(string scenarioName, string environmentName, int textEntryMaxAttempts)
    {
        ScenarioName = scenarioName;
        EnvironmentName = environmentName;
        TextEntryMaxAttempts = textEntryMaxAttempts;
        ArtifactPrefix = CreateArtifactPrefix(scenarioName);
    }

    public string ScenarioName { get; }

    public string EnvironmentName { get; }

    public int TextEntryMaxAttempts { get; }

    public string ArtifactPrefix { get; }

    public static TestRunConfig FromEnvironment()
    {
        var scenarioName = ReadValue("SCENARIO_NAME", DefaultScenarioName);
        var environmentName = ReadValue("AUTOMATION_ENV", DefaultEnvironmentName);
        var maxAttemptsValue = Environment.GetEnvironmentVariable("TEXT_ENTRY_MAX_ATTEMPTS");
        var textEntryMaxAttempts = int.TryParse(maxAttemptsValue, out var parsedAttempts) && parsedAttempts > 0
            ? parsedAttempts
            : DefaultTextEntryMaxAttempts;

        return new TestRunConfig(scenarioName, environmentName, textEntryMaxAttempts);
    }

    private static string ReadValue(string variableName, string fallback)
    {
        var value = Environment.GetEnvironmentVariable(variableName);

        return string.IsNullOrWhiteSpace(value)
            ? fallback
            : value.Trim();
    }

    private static string CreateArtifactPrefix(string scenarioName)
    {
        var normalized = Regex.Replace(scenarioName.Trim().ToLowerInvariant(), "[^a-z0-9]+", "-").Trim('-');

        return string.IsNullOrWhiteSpace(normalized)
            ? DefaultScenarioName
            : normalized;
    }
}
