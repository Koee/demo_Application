import { execFileSync } from 'child_process';
import {
  desktopProjectPath,
  desktopTestResultsPath,
  notepadTextFilePath
} from '../fixtures/paths';

// Runs the .NET/FlaUI automation process and passes the runtime paths/config from Playwright.
export function runDesktopAutomation(): string {
  return execFileSync('dotnet', ['run', '--project', desktopProjectPath], {
    encoding: 'utf-8',
    timeout: 120_000,
    windowsHide: false,
    env: {
      ...process.env,
      AUTOMATION_ENV: process.env.AUTOMATION_ENV ?? 'local',
      SCENARIO_NAME: process.env.SCENARIO_NAME ?? 'notepad-save-text',
      TEST_RESULTS_DIR: desktopTestResultsPath,
      NOTEPAD_TEXT_FILE: notepadTextFilePath
    }
  });
}
