import { defineConfig } from '@playwright/test';

export default defineConfig({
  testDir: './tests/playwright',
  outputDir: './test-results/playwright/artifacts',
  reporter: [
    ['list'],
    ['html', { outputFolder: 'test-results/playwright/report', open: 'never' }]
  ],
  use: {
    trace: 'retain-on-failure',
    screenshot: 'only-on-failure'
  }
});
