# demo_Application

Demo project for Windows desktop automation with Playwright tests and a small .NET helper app.

## Structure

```text
demo_Application/
├─ package.json
├─ playwright.config.ts
├─ tests/
│  └─ notepad-paint.spec.ts
├─ DesktopAutomation/
│  ├─ DesktopAutomation.csproj
│  └─ Program.cs
├─ .github/
│  └─ workflows/
│     └─ desktop-test.yml
└─ .vscode/
   ├─ mcp.json
   └─ settings.json
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
```
