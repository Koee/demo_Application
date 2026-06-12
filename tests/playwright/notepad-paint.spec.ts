import { expect, test } from '@playwright/test';
import { runDesktopAutomation } from '../../src/playwright/components/desktop-runner';

test('Windows app automation: Notepad -> save -> screenshot @desktop @smoke @notepad', async () => {
  const output = runDesktopAutomation();

  console.log(output);

  expect(output).toContain('RESULT: PASS');
});
