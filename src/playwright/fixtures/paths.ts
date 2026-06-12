import path from 'path';

export const repoRoot = path.resolve(__dirname, '../../..');
export const testResultsRoot = path.join(repoRoot, 'test-results');

export const desktopProjectPath = path.join(repoRoot, 'DesktopAutomation');
export const desktopTestResultsPath = path.join(testResultsRoot, 'desktop');
export const notepadTextFilePath = path.join(
  repoRoot,
  'test-data',
  'desktop',
  'notepad',
  'notepad-text.txt'
);
