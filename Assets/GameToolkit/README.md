# GameToolkit - 遊戲開發工具包

這是一套可以複製到其他 Unity 專案使用的**獨立遊戲元件**。
每個元件都經過設計，可以單獨使用或組合使用，適合用來快速開發各種類型的遊戲。

---

## 📦 包含哪些元件？

| 元件名稱 | 功能說明 | 適合用在 |
|---------|---------|---------|
| **ScoreManager** | 計分系統（加分、扣分、顯示） | 收集遊戲、射擊遊戲、任何需要計分的遊戲 |
| **CountdownTimer** | 倒數計時器（3、2、1 開始） | 競速遊戲、限時挑戰、回合制遊戲的開場 |
| **TrialCounter** | 回合計數器（追蹤第幾回合） | 心理學實驗、打地鼠、波次制遊戲 |
| **ReactionTimeRecorder** | 反應時間紀錄器（測量反應速度） | 反應測試、心理學實驗、競技遊戲 |

---

## 🚀 如何複製到新專案？

### 方法一：直接複製資料夾（推薦）

1. **找到 GameToolkit 資料夾**
   在 Unity 的 Project 視窗中找到 `Assets/GameToolkit/`

2. **複製整個資料夾**
   - 在 GameToolkit 上點右鍵 → 選擇「Show in Explorer」（Windows）或「Reveal in Finder」（Mac）
   - 複製整個 `GameToolkit` 資料夾

3. **貼到新專案**
   - 開啟你的新專案
   - 在 Unity 的 Project 視窗中，對 `Assets` 資料夾點右鍵 → 選擇「Show in Explorer / Reveal in Finder」
   - 把剛才複製的 `GameToolkit` 資料夾貼進去

4. **完成！**
   回到 Unity，它會自動偵測到新檔案並完成匯入。你現在可以在新專案中使用這些元件了。

### 方法二：只複製需要的元件

如果你只需要其中一兩個元件（例如只需要計分系統），可以：

1. 打開 `Assets/GameToolkit/Scripts/`
2. 選擇你需要的 .cs 檔案（例如 `ScoreManager.cs`）
3. 複製到新專案的任意資料夾（例如 `Assets/Scripts/`）

---

## 📖 如何使用這些元件？

### 基本步驟（通用）

1. **建立 GameObject**
   在 Hierarchy 視窗中點右鍵 → Create Empty（或使用現有的 GameObject）

2. **掛上元件**
   選中 GameObject → 在 Inspector 中點「Add Component」→ 搜尋元件名稱（例如 `ScoreManager`）

3. **設定參數**
   在 Inspector 中調整元件的參數（例如指定 UI Text、設定總回合數等）

4. **在程式碼中使用**
   參考下面各元件的使用說明，或查看 `Examples/` 資料夾中的範例腳本

---

### ScoreManager（計分系統）

**用途**：管理遊戲分數，自動更新 UI 顯示。

**設定**：
- `Score Text`：拖曳一個 UI Text 元件（用來顯示分數）
- `Display Format`：分數顯示格式，例如「分數：{0}」

**程式碼範例**：
```csharp
public ScoreManager scoreManager;  // 在 Inspector 中指定

void OnCoinCollected()
{
    scoreManager.AddScore(10);  // 加 10 分
}

void OnHitByEnemy()
{
    scoreManager.SubtractScore(5);  // 扣 5 分
}

void CheckGameOver()
{
    if (scoreManager.CurrentScore >= 100)
    {
        Debug.Log("達成目標分數！");
    }
}
```

**完整範例**：請參考 `Examples/ScoreManagerExample.cs`

---

### CountdownTimer（倒數計時器）

**用途**：在遊戲開始前顯示 3、2、1 倒數動畫。

**設定**：
- `Countdown Text`：拖曳一個 UI Text 元件（用來顯示倒數數字）
- `Count From`：從幾開始倒數（預設 3）
- `Seconds Per Count`：每個數字停留多久
- `Use Scale Animation`：是否使用縮放動畫

**程式碼範例**：
```csharp
public CountdownTimer countdownTimer;  // 在 Inspector 中指定

void StartGame()
{
    countdownTimer.StartCountdown(() => {
        Debug.Log("倒數結束！遊戲開始！");
        // 在這裡寫遊戲開始的邏輯
    });
}
```

**完整範例**：請參考 `Examples/CountdownTimerExample.cs`

---

### TrialCounter（回合計數器）

**用途**：追蹤「第幾回合 / 總共幾回合」，適合固定次數的遊戲。

**設定**：
- `Trial Text`：拖曳一個 UI Text 元件（用來顯示進度）
- `Total Trials`：總共要進行幾回合
- `Display Format`：顯示格式，例如「第 {0} / {1} 回」

**程式碼範例**：
```csharp
public TrialCounter trialCounter;  // 在 Inspector 中指定

void OnMoleClicked()
{
    trialCounter.NextTrial();  // 推進到下一回合

    if (!trialCounter.HasRemaining)
    {
        Debug.Log("所有回合完成！");
        // ShowGameOverScreen();
    }
}
```

**完整範例**：請參考 `Examples/TrialCounterExample.cs`

---

### ReactionTimeRecorder（反應時間紀錄器）

**用途**：記錄玩家的反應時間（毫秒），並計算平均值。

**設定**：
- 這個元件不需要在 Inspector 中設定任何參數，直接掛上去就能用

**程式碼範例（自動計時）**：
```csharp
public ReactionTimeRecorder rtRecorder;  // 在 Inspector 中指定

void OnMoleAppear()
{
    rtRecorder.StartTiming();  // 刺激出現時開始計時
}

void OnMoleClicked()
{
    float ms = rtRecorder.StopTimingMs();  // 玩家反應時停止計時
    Debug.Log($"反應時間：{ms:F1} 毫秒");
}

void ShowStatistics()
{
    float avg = rtRecorder.GetAverageMs();
    Debug.Log($"平均反應時間：{avg:F1} 毫秒");
}
```

**程式碼範例（手動記錄）**：
```csharp
// 如果你自己已經算好反應時間，可以直接記錄
rtRecorder.RecordTime(523.4f);  // 記錄一筆 523.4 毫秒的反應時間
```

**完整範例**：請參考 `Examples/ReactionTimeRecorderExample.cs`

---

## 🎓 範例腳本的使用方法

`Examples/` 資料夾中包含 4 個範例腳本，示範每個元件的完整用法：

| 範例腳本 | 測試方式 |
|---------|---------|
| `ScoreManagerExample.cs` | 按 A 加分、S 扣分、R 重置、Space 顯示 |
| `CountdownTimerExample.cs` | 按 Space 開始倒數 |
| `TrialCounterExample.cs` | 按 N 推進回合、R 重置、Space 顯示進度 |
| `ReactionTimeRecorderExample.cs` | 按 S 開始計時、Space 停止計時、A 顯示統計 |

**如何使用範例腳本**：

1. 建立一個空的 GameObject（在 Hierarchy 點右鍵 → Create Empty）
2. 將對應的元件和範例腳本都掛到這個 GameObject 上
3. 在 Inspector 中將元件拖曳到範例腳本的對應欄位
4. 執行遊戲並按照範例腳本註解中的按鍵測試功能

---

## ❓ 常見問題

### Q1：我可以只複製其中一個元件嗎？

可以！這些元件彼此獨立，你可以只複製需要的 .cs 檔案到新專案。

### Q2：我可以修改這些元件的程式碼嗎？

當然可以！這些元件就是設計來讓你學習和修改的。建議先複製一份再改，這樣原始版本還能保留。

### Q3：為什麼元件沒有顯示在 UI 上？

請確認你有在 Inspector 中指定 UI Text 元件。如果你不需要 UI 顯示（例如只想用程式碼讀取分數），可以把 Text 欄位留空。

### Q4：我想要改變分數顯示的樣式怎麼辦？

修改 `Display Format` 參數即可。例如：
- 中文：「分數：{0}」
- 英文：「Score: {0}」
- 只顯示數字：「{0}」

### Q5：反應時間紀錄器的時間單位是什麼？

毫秒（milliseconds）。例如 523.4 表示 0.5234 秒。

### Q6：我想把反應時間資料存成檔案，怎麼做？

使用 `GetAllRecords()` 取得所有紀錄，然後用 `System.IO.File.WriteAllText()` 寫成 CSV 檔案：

```csharp
List<float> data = rtRecorder.GetAllRecords();
string csv = string.Join(",", data);
System.IO.File.WriteAllText("reaction_times.csv", csv);
```

---

## 📁 資料夾結構

```
GameToolkit/
├── Scripts/          # 4 個可復用元件
│   ├── ScoreManager.cs
│   ├── CountdownTimer.cs
│   ├── TrialCounter.cs
│   └── ReactionTimeRecorder.cs
├── Examples/         # 使用範例腳本
│   ├── ScoreManagerExample.cs
│   ├── CountdownTimerExample.cs
│   ├── TrialCounterExample.cs
│   └── ReactionTimeRecorderExample.cs
└── README.md         # 本說明文件
```

---

## 💡 教學建議

**給老師的建議**：

1. **先示範單一元件**：例如先只教 ScoreManager，讓學生理解「掛元件 → 設定參數 → 寫程式碼呼叫」的流程

2. **用範例腳本測試**：讓學生先執行範例腳本，按按鍵看效果，理解每個方法的作用

3. **逐步修改**：讓學生修改範例腳本中的數值（例如改成加 20 分而不是 10 分），觀察結果

4. **組合使用**：熟悉單一元件後，示範如何組合多個元件（例如倒數計時器 + 計分系統 + 回合計數器）

5. **實作小專案**：讓學生用這些元件做一個簡單遊戲（例如限時收集金幣、反應測試小遊戲）

---

## 📝 版本記錄

- **v1.0**（2026-03-17）：初始版本，包含 4 個可復用元件及完整範例

---

**祝你開發順利！如果有任何問題，請參考範例腳本或詢問老師。**
