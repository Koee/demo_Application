# Huong Dan cho nguoi moi

Tai lieu nay giup nguoi moi vao du an nam nhanh cau truc, flow chay code, va vi tri can viet code khi mo rong automation.

## 1. Cau truc hien tai

```text
demo_Application/
|-- tests/playwright/              # Noi viet testcase Playwright
|-- src/playwright/components/     # Helper TypeScript de goi desktop automation
|-- src/playwright/fixtures/       # Path/config dung chung cho Playwright
|-- DesktopAutomation/
|   |-- Program.cs                 # Entry point mong
|   |-- Runners/                   # Setup, teardown, pass/fail, artifact lifecycle
|   |-- Apps/                      # Flow xu ly tung app Windows
|   `-- Components/                # Ham dung lai cho FlaUI
|-- test-data/                     # Data input cho testcase
|-- test-results/                  # Ket qua sinh ra sau khi chay test
|-- Rule-Project/                  # Rule viet code va review
`-- .github/workflows/             # CI/CD
```

Project dang theo huong:

- Playwright lam test runner va orchestration.
- FlaUI lam desktop automation layer.
- Data, test case, app flow, component dung lai, va ket qua test duoc tach rieng.

## 2. Nguoi moi nen doc tu dau

Nen doc theo thu tu sau:

1. `README.md`: nam tong quan project va command chay test.
2. `Rule-Project/CONTRIBUTOR_RULES.md`: biet rule khi them code moi.
3. `Rule-Project/REVIEW_RULES.md`: biet tieu chuan review code automation.
4. `tests/playwright/notepad-paint.spec.ts`: diem bat dau cua testcase.
5. `src/playwright/components/desktop-runner.ts`: cach Playwright goi .NET/FlaUI.
6. `src/playwright/fixtures/paths.ts`: cac path dung chung.
7. `DesktopAutomation/Program.cs`: entry point cua desktop automation.
8. `DesktopAutomation/Runners/DesktopTestRunner.cs`: setup, teardown, pass/fail, artifact.
9. `DesktopAutomation/Apps/NotepadAutomation.cs`: flow xu ly Notepad.
10. `DesktopAutomation/Components/*`: cac helper dung lai.

Flow chay hien tai:

```text
npm test
-> Playwright spec
-> desktop-runner.ts
-> dotnet run DesktopAutomation
-> Program.cs
-> DesktopTestRunner
-> NotepadAutomation
-> FlaUI Components
-> test-results/
```

## 3. Nguoi sau viet code o dau

- Viet testcase moi: `tests/playwright/`
- Viet helper TypeScript dung lai: `src/playwright/components/`
- Viet path/config TypeScript dung chung: `src/playwright/fixtures/`
- Viet flow xu ly app Window/Form: `DesktopAutomation/Apps/`
- Viet ham dung lai nhieu lan: `DesktopAutomation/Components/`
- Viet logic setup/teardown/pass/fail chung: `DesktopAutomation/Runners/`
- Them data input: `test-data/<domain>/<app>/`
- Xem ket qua test: `test-results/`
- Cap nhat rule/tai lieu: `Rule-Project/`

Vi du khi them app Paint:

```text
tests/playwright/paint.spec.ts
test-data/desktop/paint/
DesktopAutomation/Apps/PaintAutomation.cs
DesktopAutomation/Components/      # chi them helper neu co the dung lai
```

## 4. Cac folder ket qua test

```text
test-results/
|-- playwright/
|   |-- artifacts/                 # Trace/screenshot Playwright khi fail
|   `-- report/                    # HTML report
`-- desktop/
    |-- pass/                      # Screenshot/output file khi pass
    |-- false/                     # Error log/screenshot khi fail
    `-- desktop-run.log            # Log chay desktop automation
```

Khong commit file trong `test-results/`.

## 5. Quy tac quan trong khi viet automation

- Testcase Playwright khong nen chua logic thao tac Window chi tiet.
- Logic thao tac Window/Form nam trong `DesktopAutomation/Apps/`.
- Ham co kha nang dung lai nam trong `DesktopAutomation/Components/`.
- Data input khong hard-code trong testcase neu co the dat trong `test-data/`.
- Khi thao tac keyboard hoac chup hinh, phai dua window len foreground.
- Phai verify ket qua that, vi du file da save dung noi dung.
- Chi tao artifact trong `pass/` sau khi dieu kien pass da duoc xac nhan.
- Khi fail, phai co log hoac screenshot du de debug.
- Dung `AutomationLogger` cho log desktop automation.
- Dung `TestRunConfig` cho scenario name, environment, va retry setting.

## 6. Command hay dung

```bash
npm install
dotnet build DesktopAutomation/DesktopAutomation.csproj
npm test
npm run report
```

Co the loc test theo tag Playwright:

```bash
npx playwright test --grep @smoke
npx playwright test --grep @desktop
```

## 7. Luu y khi project lon hon

Khi so luong app va testcase tang, nen tiep tuc bo sung:

- Tag test ro hon: `@smoke`, `@regression`, `@desktop`, `@notepad`, `@paint`.
- Convention dat ten artifact theo scenario.
- Logging co du thong tin scenario, environment, step, expected/actual.
- Config rieng cho local va CI.
- Checklist review bat buoc truoc khi merge.
- Neu can report nang cao, can nhac them Allure hoac dashboard rieng.

Hien tai project da co nen tang tot de mo rong, nhung can giu ky luat folder va rule ngay tu dau de tranh code bi tron khi du an lon len.
