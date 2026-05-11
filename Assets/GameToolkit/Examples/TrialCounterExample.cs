using UnityEngine;

/// <summary>
/// TrialCounter 使用範例
///
/// 這個腳本示範如何在你的遊戲中使用 TrialCounter 元件。
/// 把這個腳本掛到任意 GameObject 上，然後在 Inspector 中指定 TrialCounter。
///
/// 按鍵測試：
///   - 按 N 鍵：推進到下一回合
///   - 按 R 鍵：重置回合
///   - 按 Space：顯示目前進度
/// </summary>
public class TrialCounterExample : MonoBehaviour
{
    [Header("===== 請在 Inspector 中指定 =====")]
    [Tooltip("拖曳有掛 TrialCounter 元件的物件到這裡")]
    public TrialCounter trialCounter;

    void Start()
    {
        // 範例：在遊戲開始時重置回合
        trialCounter.ResetTrials();
        Debug.Log("遊戲開始！總共 " + trialCounter.TotalTrials + " 回合");
    }

    void Update()
    {
        // 範例 1：按 N 推進回合
        if (Input.GetKeyDown(KeyCode.N))
        {
            OnTrialCompleted();
        }

        // 範例 2：按 R 重置回合
        if (Input.GetKeyDown(KeyCode.R))
        {
            trialCounter.ResetTrials();
            Debug.Log("回合已重置");
        }

        // 範例 3：按 Space 查看進度
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"目前進度：{trialCounter.CurrentTrial} / {trialCounter.TotalTrials}");
            Debug.Log($"還有剩餘回合嗎？{trialCounter.HasRemaining}");
        }
    }

    // ========== 基本使用範例 ==========

    /// <summary>
    /// 範例 1：完成一個回合的處理
    /// </summary>
    void OnTrialCompleted()
    {
        // 推進回合
        trialCounter.NextTrial();
        Debug.Log($"完成第 {trialCounter.CurrentTrial} 回合");

        // 檢查是否還有剩餘回合
        if (trialCounter.HasRemaining)
        {
            Debug.Log($"繼續下一回合（還剩 {trialCounter.TotalTrials - trialCounter.CurrentTrial} 回合）");
            // StartNextTrial();  // 開始下一回合
        }
        else
        {
            Debug.Log("所有回合已完成！遊戲結束！");
            // ShowGameOverScreen();  // 顯示結算畫面
        }
    }

    // ========== 進階使用範例 ==========

    /// <summary>
    /// 範例 2：打地鼠遊戲的回合控制
    /// </summary>
    public void OnMoleClicked()
    {
        Debug.Log("擊中地鼠！");

        // 推進回合
        trialCounter.NextTrial();

        // 檢查遊戲是否結束
        if (!trialCounter.HasRemaining)
        {
            Debug.Log("30 隻地鼠都打完了！遊戲結束！");
            // EndGame();
        }
        else
        {
            // 繼續生成下一隻地鼠
            // SpawnNextMole();
        }
    }

    /// <summary>
    /// 範例 3：動態修改總回合數
    /// </summary>
    public void SetDifficulty(string difficulty)
    {
        switch (difficulty)
        {
            case "簡單":
                trialCounter.TotalTrials = 10;
                break;
            case "普通":
                trialCounter.TotalTrials = 30;
                break;
            case "困難":
                trialCounter.TotalTrials = 50;
                break;
        }

        trialCounter.ResetTrials();
        Debug.Log($"難度設為：{difficulty}（{trialCounter.TotalTrials} 回合）");
    }

    /// <summary>
    /// 範例 4：根據回合數調整難度
    /// </summary>
    public void StartNextWave()
    {
        trialCounter.NextTrial();

        // 每 5 回合提升難度
        if (trialCounter.CurrentTrial % 5 == 0)
        {
            Debug.Log($"第 {trialCounter.CurrentTrial} 波！難度提升！");
            // IncreaseEnemySpeed();
            // IncreaseEnemyCount();
        }
    }

    /// <summary>
    /// 範例 5：顯示進度百分比
    /// </summary>
    public void ShowProgress()
    {
        float progress = (float)trialCounter.CurrentTrial / trialCounter.TotalTrials * 100f;
        Debug.Log($"遊戲進度：{progress:F1}%");
    }
}
