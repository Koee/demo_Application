# Review Rules For Senior QA Automation

Use this checklist when reviewing code written by Codex, a tester, or another automation engineer.

## Review Mindset

Review as a Senior QA Automation engineer. Focus on correctness, maintainability, reliability, observability, and long-term scalability.

Prioritize issues that the author can fix clearly:

- Incorrect test behavior or false pass/false fail risk.
- Flaky desktop interaction.
- Mixed responsibilities between test, runner, app flow, and shared component layers.
- Missing result artifacts or confusing artifact paths.
- Poor data separation.
- Missing verification of important UI or file side effects.
- CI behavior that differs from local behavior.
- Missing or unclear logging for desktop failures.

Avoid low-value style comments unless they affect maintainability.

## Architecture Checklist

- Playwright specs stay in `tests/playwright/`.
- Playwright helpers stay in `src/playwright/components/`.
- Shared Playwright config/path fixtures stay in `src/playwright/fixtures/`.
- FlaUI app flows stay in `DesktopAutomation/Apps/`.
- FlaUI reusable utilities stay in `DesktopAutomation/Components/`.
- Runner lifecycle and pass/fail handling stay in `DesktopAutomation/Runners/`.
- Input data stays in `test-data/`.
- Generated outputs stay in `test-results/`.

Flag changes that put logic into the wrong layer.

## Desktop Reliability Checklist

- The target app process is cleaned up before and after the test.
- The target window is activated before typing or capture.
- Text input is robust against keyboard layout or IME issues.
- The automation verifies saved data or final UI state.
- Failure paths capture useful screenshots and error logs.
- Pass artifacts are not created before the scenario has actually passed.
- Sleeps are minimal and justified; prefer waits/retries when possible.
- Scenario names and retry settings come from `TestRunConfig`, not scattered constants.

## Artifact Checklist

- Playwright report is written under `test-results/playwright/report/`.
- Playwright trace/screenshots are written under `test-results/playwright/artifacts/`.
- Desktop pass artifacts are written under `test-results/desktop/pass/`.
- Desktop failure artifacts are written under `test-results/desktop/false/`.
- Desktop execution logs are written to `test-results/desktop/desktop-run.log`.
- CI uploads the same folders used locally.
- No generated artifacts are committed.

## Test Quality Checklist

- A test has a clear scenario name.
- A test has useful tags such as `@desktop`, `@smoke`, `@regression`, or app-specific tags.
- A test validates a real result, not only that a command exited.
- Test data is externalized when it is business/input data.
- Reusable logic is extracted only when it is actually shared.
- Error messages explain the expected and actual behavior.
- Tests remain readable for a new QA engineer joining the project.

## Review Output Format

For each issue, include:

- Priority: `P1`, `P2`, or `P3`.
- Location: file and line/function.
- Problem: what can go wrong.
- Fix suggestion: what the author should change.

If there are no actionable issues, say that clearly and mention any remaining risk.
