# demo_Application

Demo project for Windows desktop automation with Playwright as the orchestration test runner and FlaUI as the Windows UI automation layer.

## Structure

```text
demo_Application/
|-- package.json
|-- playwright.config.ts
|-- tests/
|   `-- playwright/
|       `-- notepad-paint.spec.ts          # Test cases only
|-- src/
|   `-- playwright/
|       |-- components/                    # Reusable Playwright runner/helpers
|       `-- fixtures/                      # Shared paths/config for tests
|-- DesktopAutomation/
|   |-- DesktopAutomation.csproj
|   |-- Program.cs                         # Thin entry point
|   |-- Runners/                           # Test runner/pass-fail orchestration
|   |-- Apps/                              # App-specific desktop flows
|   `-- Components/                        # Reusable FlaUI utilities
|-- test-data/
|   `-- desktop/
|       `-- notepad/
|           `-- notepad-text.txt           # Input data
|-- test-results/                          # Generated reports/results, ignored by Git
|   |-- playwright/
|   |   |-- artifacts/                     # Playwright trace/screenshots on failure
|   |   `-- report/                        # Playwright HTML report
|   `-- desktop/
|       |-- pass/                          # Desktop screenshots and saved output files
|       |-- false/                         # Desktop error logs and failure screenshots
|       `-- work/                          # Temporary files during desktop execution
|-- Rule-Project/
|   |-- CONTRIBUTOR_RULES.md               # Rules for adding automation code
|   |-- REVIEW_RULES.md                    # Rules for reviewing automation code
|   `-- Hướng Dẫn cho người mới.md         # Onboarding guide for new contributors
|-- .github/
|   `-- workflows/
|       `-- desktop-test.yml
`-- .vscode/
    |-- mcp.json
    `-- settings.json
```

## Layering

```text
Playwright Test Runner
|-- tests/playwright            # Test structure
|-- test-results/playwright     # HTML report, trace, screenshot, Playwright artifacts
|-- test-results/desktop        # Desktop screenshots, saved files, error logs
|-- src/playwright/components   # Command line orchestration/setup helpers
|-- .github/workflows           # CI/CD pipeline
`-- Rule-Project                # Contributor and review rules

FlaUI Desktop Automation
|-- DesktopAutomation/Apps       # WinForms/WPF/Win32 navigation flows
|-- DesktopAutomation/Components # Click/type/read/capture/process helpers
|-- DesktopAutomation/Runners    # Desktop pass/fail and artifact handling
`-- test-data/desktop            # Desktop input data
```

## GitHub

The project includes `.github/workflows/desktop-test.yml` for running the desktop test workflow on `windows-latest`.

Before pushing to GitHub, update the repository fields in `package.json`:

```json
{
  "repository": {
    "type": "git",
    "url": "git+https://github.com/your-org/demo_Application.git"
  }
}
```

## Playwright MCP

The project includes `.vscode/mcp.json` with the Playwright MCP server:

```json
{
  "servers": {
    "playwright": {
      "command": "npx",
      "args": ["@playwright/mcp@latest"],
      "type": "stdio"
    }
  }
}
```

## Commands

```bash
npm install
npm test
npm run test:smoke
npm run test:desktop
npm run report
```

## Runtime Config

```text
AUTOMATION_ENV=local|ci
SCENARIO_NAME=notepad-save-text
TEXT_ENTRY_MAX_ATTEMPTS=3
```

Desktop automation writes logs to `test-results/desktop/desktop-run.log`.
