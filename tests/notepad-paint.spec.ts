import { test, expect } from '@playwright/test';
import { execFileSync } from 'child_process';
import path from 'path';

test('Windows app automation: Notepad -> save -> screenshot', async () => {
  const projectPath = path.resolve(__dirname, '../DesktopAutomation');
  const testResultsPath = path.resolve(__dirname, '../test-results');

  const output = execFileSync(
    'dotnet',
    ['run', '--project', projectPath],
    {
      encoding: 'utf-8',
      timeout: 120_000,
      windowsHide: false,
      env: {
        ...process.env,
        TEST_RESULTS_DIR: testResultsPath
      }
    }
  );

  console.log(output);

  expect(output).toContain('RESULT: PASS');
});
