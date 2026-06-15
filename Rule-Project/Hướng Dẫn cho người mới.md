# Huong Dan Cho Nguoi Moi

Tai lieu nay giup intern hoac nguoi moi tham gia project co the doc flow code nhanh nhat va bat dau sua task nho ma khong can hieu het toan bo source ngay tu dau.

Muc tieu sau khi doc xong:

- Biet project nay chay test nhu the nao.
- Biet Playwright va FlaUI chia viec ra sao.
- Biet doc code theo thu tu nao de khong bi roi.
- Biet khi them test, them app, them data, sua helper thi vao folder nao.
- Biet command nao can chay truoc khi gui review.

Project hien tai la automation Windows desktop:

```text
Playwright + TypeScript -> dung lam test runner/orchestration
.NET 8 + FlaUI          -> dung de thao tac UI Windows desktop
```

---

## 1. Ban Can Hieu 1 Y Tuong Chinh

Code trong project duoc chia theo lop:

```text
tests/playwright/              -> noi khai bao testcase Playwright
src/playwright/components/     -> helper TypeScript de goi desktop automation
src/playwright/fixtures/       -> path/config dung chung cho Playwright
DesktopAutomation/Program.cs   -> entry point mong cua .NET app
DesktopAutomation/Runners/     -> setup, teardown, pass/fail, artifact lifecycle
DesktopAutomation/Apps/        -> flow automation rieng cho tung app Windows
DesktopAutomation/Components/  -> helper FlaUI dung chung
test-data/                     -> input data cho testcase
test-results/                  -> report, screenshot, log, output sinh ra khi chay test
Rule-Project/                  -> rule viet code, review, onboarding
```

Hay nho cong thuc nay:

```text
Spec khong thao tac desktop chi tiet.
Spec chi goi TypeScript runner va assert ket qua ngoai.
TypeScript runner chay .NET desktop automation.
.NET runner quan ly lifecycle, pass/fail, artifact.
App automation xu ly flow cua tung ung dung Windows.
Component chua thao tac dung chung nhu find window, activate, input, capture, log.
Data nam trong test-data.
Ket qua sinh ra nam trong test-results.
```

Flow hien tai cua test Notepad:

```text
tests/playwright/notepad-paint.spec.ts
  -> src/playwright/components/desktop-runner.ts
      -> dotnet run --project DesktopAutomation
          -> DesktopAutomation/Program.cs
              -> DesktopAutomation/Runners/DesktopTestRunner.cs
                  -> DesktopAutomation/Apps/NotepadAutomation.cs
                      -> DesktopAutomation/Components/*
                          -> test-data/desktop/notepad/notepad-text.txt
                          -> test-results/desktop/*
```

---

## 2. 30 Phut Dau Nen Doc Gi

Dung thu tu nay. Dung doc lan man het project ngay tu dau.

### Buoc 1: Doc tong quan va command

Doc:

```text
README.md
package.json
playwright.config.ts
```

Can nam:

- Lenh nao de chay test.
- Playwright dang tim test o folder nao.
- Playwright report, trace, screenshot ghi vao dau.
- Script `npm test`, `npm run test:smoke`, `npm run test:desktop`, `npm run report` lam gi.

### Buoc 2: Doc rule truoc khi sua code

Doc:

```text
Rule-Project/CONTRIBUTOR_RULES.md
Rule-Project/REVIEW_RULES.md
```

Can nam:

- Logic nao duoc dat trong spec.
- Logic nao phai dat trong `DesktopAutomation/Apps/`.
- Helper dung chung nam o dau.
- Artifact pass/fail phai ghi vao dau.
- Reviewer se soi nhung loi nao.

### Buoc 3: Doc spec de biet test bat dau tu dau

Doc:

```text
tests/playwright/notepad-paint.spec.ts
```

Can nam:

- Test Playwright hien tai chi goi `runDesktopAutomation()`.
- Test assert output co chuoi `RESULT: PASS`.
- Tag hien tai: `@desktop`, `@smoke`, `@notepad`.
- Neu spec bat dau chua nhieu logic thao tac desktop, do la dau hieu can tach xuong .NET/FlaUI layer.

### Buoc 4: Doc TypeScript runner

Doc:

```text
src/playwright/components/desktop-runner.ts
src/playwright/fixtures/paths.ts
```

Can nam:

- `desktop-runner.ts` goi `dotnet run --project DesktopAutomation`.
- Env duoc truyen vao .NET gom `AUTOMATION_ENV`, `SCENARIO_NAME`, `TEST_RESULTS_DIR`, `NOTEPAD_TEXT_FILE`.
- `paths.ts` tinh path tu repo root, tranh hardcode absolute path local.

### Buoc 5: Doc .NET entry va runner

Doc:

```text
DesktopAutomation/Program.cs
DesktopAutomation/Runners/DesktopTestRunner.cs
```

Can nam:

- `Program.cs` chi la entry point mong, khong nen nhet flow app vao day.
- `DesktopTestRunner` quan ly artifact, logger, kiem tra Windows, cleanup process, pass/fail, failure screenshot.
- Khi fail, error duoc ghi vao `test-results/desktop/false/error.txt`.

### Buoc 6: Doc app flow hien tai

Doc:

```text
DesktopAutomation/Apps/NotepadAutomation.cs
```

Can nam:

- Flow mo Notepad.
- Doc input text tu test data.
- Paste text vao editor.
- Save file.
- Verify file da save dung noi dung.
- Chi tao pass artifact sau khi verify pass.
- Capture screenshot va output file vao `test-results/desktop/pass/`.

### Buoc 7: Chi doc helper khi can sua logic dung chung

Doc khi can:

```text
DesktopAutomation/Components/ArtifactPaths.cs
DesktopAutomation/Components/AutomationLogger.cs
DesktopAutomation/Components/ProcessManager.cs
DesktopAutomation/Components/TestDataReader.cs
DesktopAutomation/Components/TestRunConfig.cs
DesktopAutomation/Components/TextInput.cs
DesktopAutomation/Components/WindowActivator.cs
DesktopAutomation/Components/WindowCapture.cs
DesktopAutomation/Components/WindowFinder.cs
```

Khong nen sua helper dung chung neu chua chac, vi helper co the anh huong nhieu app sau nay.

---

## 3. Cach Chay Project

Cai dependency Node:

```bash
npm install
```

Build .NET desktop automation:

```bash
dotnet build DesktopAutomation/DesktopAutomation.csproj
```

Chay tat ca test Playwright:

```bash
npm test
```

Chay smoke test:

```bash
npm run test:smoke
```

Chay desktop test:

```bash
npm run test:desktop
```

Chay headed neu can quan sat browser runner cua Playwright:

```bash
npm run test:headed
```

Mo Playwright HTML report:

```bash
npm run report
```

Chay theo tag:

```bash
npx playwright test --grep @smoke
npx playwright test --grep @desktop
npx playwright test --grep @notepad
```

Chay truc tiep .NET automation neu muon debug layer desktop:

```bash
dotnet run --project DesktopAutomation
```

Luu y:

- Desktop automation can Windows vi project dung FlaUI de thao tac app Windows.
- Test hien tai se kill process `notepad` truoc va sau khi chay.
- Khong commit file sinh ra trong `test-results/`.

---

## 4. Hieu Flow Notepad Hien Tai

Entry point cua test:

```text
tests/playwright/notepad-paint.spec.ts
```

Spec goi:

```text
runDesktopAutomation()
```

Function nam o:

```text
src/playwright/components/desktop-runner.ts
```

Thu tu flow:

```text
1. Playwright chay spec `notepad-paint.spec.ts`.
2. Spec goi `runDesktopAutomation()`.
3. TypeScript runner goi `dotnet run --project DesktopAutomation`.
4. Runner truyen env/path can thiet vao .NET.
5. `Program.cs` goi `DesktopTestRunner.Run()`.
6. `DesktopTestRunner` tao artifact path va config.
7. Runner clean output root cu trong `test-results/desktop/`.
8. Runner cau hinh `AutomationLogger`.
9. Runner kiem tra OS phai la Windows.
10. Runner kill Notepad cu de tranh nhieu window gay nhieu.
11. Runner goi `NotepadAutomation.Run()`.
12. Notepad flow doc text tu `test-data/desktop/notepad/notepad-text.txt`.
13. Flow mo Notepad voi file tam trong `test-results/desktop/work/`.
14. Flow focus window, paste text, save file.
15. Flow doc lai file da save de verify noi dung.
16. Neu pass, flow copy output va screenshot vao `test-results/desktop/pass/`.
17. Runner log `RESULT: PASS`.
18. Playwright assert output co `RESULT: PASS`.
19. Cuoi cung runner kill Notepad va clean work dir.
```

File phu trach tung phan:

```text
notepad-paint.spec.ts
  - Khai bao testcase
  - Goi runner
  - Assert output co RESULT: PASS

desktop-runner.ts
  - Goi dotnet run
  - Truyen env/path vao .NET process
  - Set timeout 120 giay

paths.ts
  - Tinh repo root
  - Tinh path DesktopAutomation
  - Tinh path test-results/desktop
  - Tinh path test-data/desktop/notepad/notepad-text.txt

Program.cs
  - Entry point mong
  - Goi DesktopTestRunner

DesktopTestRunner.cs
  - Clean artifact cu
  - Cau hinh log
  - Kiem tra Windows
  - Cleanup process Notepad
  - Goi app automation
  - Xu ly pass/fail
  - Chup screenshot khi fail

NotepadAutomation.cs
  - Mo Notepad
  - Nhap text
  - Save file
  - Verify output
  - Tao pass artifact

Components/*
  - Helper dung chung cho window, input, capture, logging, data, config
```

---

## 5. Cac Folder Ket Qua Test

Sau khi chay test, xem ket qua o:

```text
test-results/
|-- playwright/
|   |-- artifacts/                 -> trace/screenshot Playwright khi fail
|   `-- report/                    -> HTML report cua Playwright
`-- desktop/
    |-- pass/                      -> screenshot/output file khi desktop flow pass
    |-- false/                     -> error log/screenshot khi desktop flow fail
    |-- work/                      -> file tam trong luc chay, se duoc cleanup
    `-- desktop-run.log            -> log chi tiet cua .NET desktop automation
```

Khi debug:

- Neu Playwright fail, xem `test-results/playwright/report/`.
- Neu .NET desktop automation fail, xem `test-results/desktop/desktop-run.log`.
- Neu co exception, xem `test-results/desktop/false/error.txt`.
- Neu app da mo duoc window truoc khi fail, xem screenshot trong `test-results/desktop/false/`.
- Neu pass, xem file output va screenshot trong `test-results/desktop/pass/`.

Khong commit `test-results/`.

---

## 6. Ban Muon Lam Viec Gi Thi Bat Dau O Dau

### Them testcase Playwright moi

Sua hoac tao:

```text
tests/playwright/<feature-or-app>.spec.ts
src/playwright/components/<runner-or-helper>.ts
```

Quy tac:

- Spec chi khai bao test, tag, goi runner/helper, va assert ket qua ngoai.
- Khong viet logic click/type desktop chi tiet trong spec.
- Dat tag ro: `@desktop`, `@smoke`, `@regression`, `@notepad`, `@paint`.

### Them flow cho app Windows moi

Vi du them Paint:

```text
tests/playwright/paint.spec.ts
DesktopAutomation/Apps/PaintAutomation.cs
test-data/desktop/paint/
```

Neu can runner chon app theo scenario, sua them:

```text
DesktopAutomation/Runners/DesktopTestRunner.cs
src/playwright/components/desktop-runner.ts
```

Quy tac:

- Flow rieng cua app nam trong `DesktopAutomation/Apps/`.
- Helper dung chung moi dua vao `DesktopAutomation/Components/`.
- Data input dat trong `test-data/desktop/<app>/`.
- Artifact dat theo scenario name de de truy vet.

### Sua flow Notepad hien tai

Sua:

```text
DesktopAutomation/Apps/NotepadAutomation.cs
```

Neu thay doi input data:

```text
test-data/desktop/notepad/notepad-text.txt
DesktopAutomation/Components/TestDataReader.cs
src/playwright/fixtures/paths.ts
src/playwright/components/desktop-runner.ts
```

Quy tac:

- Doi thu tu buoc Notepad thi sua app flow.
- Doi cach doc test data thi sua `TestDataReader`.
- Doi path test data thi sua `paths.ts` va env trong `desktop-runner.ts`.

### Sua lifecycle pass/fail, cleanup, artifact

Sua:

```text
DesktopAutomation/Runners/DesktopTestRunner.cs
DesktopAutomation/Components/ArtifactPaths.cs
```

Quy tac:

- Cleanup process dat trong runner hoac component dung chung.
- Khi fail phai de lai log/error/screenshot du de debug.
- Pass artifact chi duoc tao sau khi da verify ket qua that.

### Them helper FlaUI dung chung

Dat trong:

```text
DesktopAutomation/Components/
```

Vi du:

```text
WindowFinder.cs       -> tim main window
WindowActivator.cs    -> dua window len foreground
WindowCapture.cs      -> chup screenshot window
TextInput.cs          -> nhap/paste text vao editor
ProcessManager.cs     -> cleanup process
AutomationLogger.cs   -> ghi log ra console va file
```

Quy tac:

- Chi tach helper neu logic dung lai duoc cho 2 noi tro len hoac co y nghia dung chung ro.
- Khong dua logic nghiep vu cua rieng Notepad/Paint vao helper dung chung.

### Them data test

Dung folder:

```text
test-data/desktop/<app>/
```

Vi du:

```text
test-data/desktop/notepad/notepad-text.txt
test-data/desktop/paint/sample-image.png
```

Quy tac:

- Data nho, de doc, phuc vu dung scenario.
- Khong hardcode input business data trong spec neu co the dat trong `test-data/`.
- Khong commit secret, credential, token.

### Sua config runtime

Doc:

```text
DesktopAutomation/Components/TestRunConfig.cs
src/playwright/components/desktop-runner.ts
```

Env hien tai:

```text
AUTOMATION_ENV              -> mac dinh local
SCENARIO_NAME               -> mac dinh notepad-save-text
TEXT_ENTRY_MAX_ATTEMPTS     -> mac dinh 3
TEST_RESULTS_DIR            -> path test-results/desktop
NOTEPAD_TEXT_FILE           -> path file input Notepad
```

Quy tac:

- Khong hardcode absolute path may local.
- Gia tri phu thuoc moi truong nen di qua env/config.
- Scenario name nen ro nghia vi du `notepad-save-text`.

---

## 7. Cac Helper Quan Trong

### `DesktopAutomation/Components/ArtifactPaths.cs`

Dung de quan ly output:

- `PassDir`: `test-results/desktop/pass`
- `FalseDir`: `test-results/desktop/false`
- `WorkDir`: `test-results/desktop/work`
- `LogPath`: `test-results/desktop/desktop-run.log`

Doc file nay khi can doi cau truc artifact.

### `DesktopAutomation/Components/AutomationLogger.cs`

Dung de log ra console va file:

- `Step()` cho buoc automation quan trong.
- `Info()` cho thong tin binh thuong.
- `Error()` cho loi.

Khong nen dung `Console.WriteLine` truc tiep trong desktop automation neu log do can debug ve sau.

### `DesktopAutomation/Components/ProcessManager.cs`

Dung de kill process theo ten, hien tai dung cho `notepad`.

Doc file nay khi test bi fail vi app cu con mo hoac nhieu window gay nhieu.

### `DesktopAutomation/Components/TestDataReader.cs`

Dung de doc input data cho app.

Hien tai:

- Doc `NOTEPAD_TEXT_FILE` tu env neu co.
- Neu khong co env, tu tim `test-data/desktop/notepad/notepad-text.txt`.
- Neu file khong ton tai hoac rong, fallback ve text mac dinh.

### `DesktopAutomation/Components/TestRunConfig.cs`

Dung de doc runtime config:

- Scenario name.
- Environment name.
- So lan retry khi nhap text.
- Artifact prefix tu scenario name.

Doc file nay khi can them config moi cho desktop automation.

### `DesktopAutomation/Components/TextInput.cs`

Dung de focus editor va paste text.

Hien tai helper:

- Bring window len foreground.
- Tim editor theo control type `Document` hoac `Edit`.
- Set clipboard.
- Gui `Ctrl+A`, `Ctrl+V`.

Doc file nay khi fail vi text khong vao app, IME/keyboard layout, focus, hoac clipboard.

### `DesktopAutomation/Components/WindowActivator.cs`

Dung de dua window len foreground truoc khi input hoac capture.

Neu test flaky vi go phim vao sai window, doc file nay va flow dang goi no.

### `DesktopAutomation/Components/WindowCapture.cs`

Dung de chup screenshot window theo bounding rectangle.

Doc file nay khi artifact screenshot sai kich thuoc, den, hoac chup nham vi window chua foreground.

### `DesktopAutomation/Components/WindowFinder.cs`

Dung de doi main window cua app xuat hien.

Doc file nay khi app launch thanh cong nhung test bao khong tim thay window.

---

## 8. Quy Tac Viet Code De Khong Bi Review Lai Nhieu

Luon lam:

- Dat test name ro scenario va co tag.
- Giu spec Playwright mong, de doc.
- Dat flow thao tac app trong `DesktopAutomation/Apps/`.
- Dat helper dung chung trong `DesktopAutomation/Components/`.
- Dua window len foreground truoc khi type, paste, capture.
- Verify ket qua that, vi du file da save dung noi dung.
- Chi tao pass artifact sau khi pass condition da duoc xac nhan.
- Khi fail, phai co log/error/screenshot du de debug.
- Dung `AutomationLogger` cho log desktop automation.
- Dung `TestRunConfig` cho scenario/environment/retry.
- Chay `dotnet build DesktopAutomation/DesktopAutomation.csproj` va `npm test` truoc khi bao xong.

Khong lam:

- Khong nhet flow app vao `Program.cs`.
- Khong viet click/type desktop chi tiet trong spec Playwright.
- Khong hardcode absolute path local.
- Khong commit file trong `test-results/`.
- Khong tao pass screenshot/output truoc khi verify pass.
- Khong them helper dung chung neu logic moi chi dung 1 noi.
- Khong nuot exception lam test false pass.
- Khong log secret, credential, token.

---

## 9. Checklist Khi Intern Nhan Task

Truoc khi code:

- Task thuoc lop nao: Playwright spec, TypeScript runner, .NET runner, app flow, component, data, report?
- Entry spec la file nao?
- Flow app nam trong class nao?
- Co helper nao dung lai duoc chua?
- Ket qua mong doi can verify bang gi: text file, screenshot, log, UI state, process output?
- Artifact pass/fail can nam o dau?

Trong khi code:

- Sua dung lop trach nhiem.
- Neu chi doi flow cua Notepad, sua `NotepadAutomation.cs`.
- Neu them app moi, tao class moi trong `DesktopAutomation/Apps/`.
- Neu logic lap lai cho nhieu app, can nhac helper trong `Components/`.
- Neu data thay doi theo scenario, dua vao `test-data/` hoac env/config.
- Ghi log cac buoc quan trong bang `AutomationLogger`.

Truoc khi gui review:

```bash
dotnet build DesktopAutomation/DesktopAutomation.csproj
npm test
```

Neu chi muon chay nhanh theo tag:

```bash
npm run test:smoke
npm run test:desktop
```

Sau khi chay, kiem tra:

```text
test-results/playwright/report/
test-results/playwright/artifacts/
test-results/desktop/pass/
test-results/desktop/false/
test-results/desktop/desktop-run.log
```

Dam bao khong commit `test-results/`.

---

## 10. Loi Thuong Gap Va Cach Tu Kiem Tra

### `dotnet` khong chay hoac build fail

Kiem tra:

- May da cai .NET SDK phu hop chua.
- Chay `dotnet build DesktopAutomation/DesktopAutomation.csproj`.
- File `.csproj` dang target `net8.0-windows`.

### Test fail vi khong phai Windows

Project dung FlaUI nen desktop automation can Windows.

Kiem tra:

- Dang chay tren Windows local hoac runner Windows.
- CI dung `windows-latest`.

### Khong tim thay Notepad window

Kiem tra:

- Notepad co bi policy/permission chan khong.
- `WindowFinder.WaitForMainWindow()` co timeout qua ngan khong.
- Co process Notepad cu lam nhieu khong.
- Log trong `test-results/desktop/desktop-run.log`.

### Text khong duoc nhap vao Notepad

Kiem tra:

- Window da foreground chua.
- `TextInput.ReplaceTextInEditor()` co tim duoc editor khong.
- Clipboard co bi app khac can thiep khong.
- Keyboard layout/IME co gay anh huong khong.
- Retry `TEXT_ENTRY_MAX_ATTEMPTS` co can tang tam thoi de debug khong.

### Saved text mismatch

Kiem tra:

- File input `test-data/desktop/notepad/notepad-text.txt` co dung noi dung khong.
- `NOTEPAD_TEXT_FILE` co tro den file khac khong.
- Output file trong `test-results/desktop/work/` co bi cleanup truoc khi debug khong.
- Log co ghi actual text mismatch khong.

### Playwright fail du .NET da chay

Kiem tra:

- Output .NET co `RESULT: PASS` khong.
- `runDesktopAutomation()` co timeout 120 giay khong.
- Exception tu `execFileSync` co lam spec fail truoc assertion khong.
- Report Playwright trong `test-results/playwright/report/`.

### Artifact khong thay o pass/false

Kiem tra:

- `TEST_RESULTS_DIR` co bi override khong.
- `ArtifactPaths.FromEnvironment()` dang tro den folder nao.
- Flow co tao pass artifact sau khi verify pass khong.
- Fail co xay ra truoc khi window duoc gan vao `CurrentWindow` khong.

---

## 11. Ket Luan Ngan

Neu moi vao project, hay di theo duong nay:

```text
README.md
  -> package.json
  -> playwright.config.ts
  -> Rule-Project/CONTRIBUTOR_RULES.md
  -> Rule-Project/REVIEW_RULES.md
  -> tests/playwright/notepad-paint.spec.ts
  -> src/playwright/components/desktop-runner.ts
  -> src/playwright/fixtures/paths.ts
  -> DesktopAutomation/Program.cs
  -> DesktopAutomation/Runners/DesktopTestRunner.cs
  -> DesktopAutomation/Apps/NotepadAutomation.cs
  -> DesktopAutomation/Components/* khi can
```

Khi viet code moi:

```text
Spec goi runner.
Runner TypeScript goi .NET.
.NET runner quan ly lifecycle va artifact.
App automation xu ly flow cua ung dung Windows.
Component xu ly helper dung chung.
Data nam trong test-data.
Ket qua nam trong test-results.
```

Chi can giu dung flow nay, intern co the bat dau sua task nho ngay: doc spec de biet test nao chay, doc runner de biet app nao duoc goi, doc app automation de sua flow, va chi sua component khi logic do that su dung chung.
