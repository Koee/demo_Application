import { expect, test } from '@playwright/test';
import { runDesktopAutomation } from '../../src/playwright/components/desktop-runner';

test('Windows app automation: Notepad -> save -> screenshot @desktop @smoke @notepad', async () => {
  // Entry test: Playwright only starts the desktop runner and verifies the final process result.
  const output = runDesktopAutomation();

  console.log(output);

  expect(output).toContain('RESULT: PASS');
});
