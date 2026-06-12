# Contributor Rules

These rules guide new contributors when adding or changing automation code in this project.

## Project Direction

This project uses Playwright as the test orchestration layer and FlaUI as the Windows desktop automation layer.

- Playwright owns test cases, command execution, report configuration, and CI orchestration.
- FlaUI owns Windows app navigation, control interaction, screenshots, and desktop-specific result handling.
- Test data, reusable components, app flows, and generated results must stay in separate folders.

## Where To Put Code

Use these folders consistently:

- `tests/playwright/`: Playwright spec files. Put test cases here only.
- `src/playwright/components/`: reusable TypeScript orchestration helpers.
- `src/playwright/fixtures/`: shared TypeScript paths/config used by Playwright tests.
- `DesktopAutomation/Apps/`: app-specific flows such as Notepad, Paint, or another Windows app.
- `DesktopAutomation/Components/`: reusable FlaUI utilities such as window activation, text input, capture, process cleanup, and artifact paths.
- `DesktopAutomation/Runners/`: pass/fail orchestration and lifecycle handling.
- `test-data/`: input data used by tests.
- `test-results/`: generated reports, screenshots, logs, and output files. Do not commit files from this folder.
- `Rule-Project/`: project rules and review standards.

## Adding A New Test Case

Follow this flow:

1. Add or update test data under `test-data/<domain>/<app>/`.
2. Add app-specific automation under `DesktopAutomation/Apps/`.
3. Reuse shared helpers from `DesktopAutomation/Components/`.
4. If the test needs a new C# execution mode, add orchestration in `DesktopAutomation/Runners/`.
5. Add the Playwright spec under `tests/playwright/`.
6. Keep Playwright assertions focused on the external result: process output, result files, screenshots, or logs.
7. Add clear Playwright tags such as `@desktop`, `@smoke`, `@regression`, and the app name.

## Desktop Automation Rules

- Do not place app-specific business flow inside `Program.cs`; keep it as a thin entry point.
- Do not put reusable helpers inside app flow classes.
- Prefer explicit waits or retry helpers over blind sleeps. Short sleeps are acceptable only when Windows UI focus needs stabilization.
- Always bring the target window to the foreground before sending keyboard input or capturing screenshots.
- Verify important side effects, such as saved files or visible UI state.
- Save pass artifacts only after the pass condition is confirmed.
- Save failure artifacts under `test-results/desktop/false/`.
- Keep screenshots, logs, and output files deterministic and easy to inspect.
- Use `AutomationLogger` instead of direct `Console.WriteLine` in desktop automation code.
- Use `TestRunConfig` for scenario name, environment name, and retry settings.

## Test Data Rules

- Do not hard-code test data in test cases when it can live in `test-data/`.
- Group data by domain and app, for example `test-data/desktop/notepad/notepad-text.txt`.
- Keep test data small, readable, and purpose-specific.
- Use environment variables only for overriding paths in CI or special runs.

## Naming Rules

- Spec files: `<feature-or-app>.spec.ts`.
- App automation classes: `<AppName>Automation.cs`.
- Shared components: name by capability, for example `WindowCapture`, `TextInput`, `ArtifactPaths`.
- Result files: include the app or scenario name, for example `notepad-screenshot.png`.
- Prefer scenario-based artifact names, for example `notepad-save-text-screenshot.png`.

## Configuration Rules

- Use `AUTOMATION_ENV` to identify the current environment. Default is `local`.
- Use `SCENARIO_NAME` to control the scenario name and artifact prefix.
- Use `TEXT_ENTRY_MAX_ATTEMPTS` only when a run needs a different desktop input retry count.
- Do not hard-code local-only absolute paths.

## Logging Rules

- Desktop automation logs must be written to `test-results/desktop/desktop-run.log`.
- Logs should include setup, scenario, important steps, verification, artifact paths, and failures.
- Do not log secrets, credentials, or large test data payloads.

## Before Commit

Run these checks:

```bash
dotnet build DesktopAutomation/DesktopAutomation.csproj
npm test
```

Then verify generated files are under:

```text
test-results/playwright/
test-results/desktop/
```

Do not commit generated files from `test-results/`.
