using UnityEngine;

/// <summary>
/// CountdownTimer 使用範例
///
/// 這個腳本示範如何在你的遊戲中使用 CountdownTimer 元件。
/// 把這個腳本掛到任意 GameObject 上，然後在 Inspector 中指定 CountdownTimer。
///
/// 按鍵測試：
///   - 按 Space 鍵：開始倒數
///   - 倒數結束後會自動執行 OnCountdownFinished() 方法
/// </summary>
public class CountdownTimerExample : MonoBehaviour
{
    [Header("===== 請在 Inspector 中指定 =====")]
    [Tooltip("拖曳有掛 CountdownTimer 元件的物件到這裡")]
    public CountdownTimer countdownTimer;

    void Update()
    {
        // 範例：按 Space 開始倒數
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!countdownTimer.IsCountingDown)
            {
                StartGameCountdown();
            }
            else
            {
                Debug.Log("倒數進行中，請稍候...");
            }
        }
    }

    // ========== 基本使用範例 ==========

    /// <summary>
    /// 範例 1：簡單的倒數 + 遊戲開始
    /// </summary>
    void StartGameCountdown()
    {
        Debug.Log("準備開始倒數...");

        // 開始倒數，倒數結束後會執行 OnCountdownFinished
        countdownTimer.StartCountdown(OnCountdownFinished);
    }

    /// <summary>
    /// 倒數結束後執行的動作
    /// </summary>
    void OnCountdownFinished()
    {
        Debug.Log("倒數結束！遊戲開始！");

        // 在這裡寫遊戲開始的邏輯，例如：
        // - 開始生成敵人
        // - 開始計時
        // - 啟動玩家控制
    }

    // ========== 進階使用範例 ==========

    /// <summary>
    /// 範例 2：倒數結束後開始生成敵人
    /// </summary>
    public void StartLevel()
    {
        countdownTimer.StartCountdown(() =>
        {
            Debug.Log("關卡開始！開始生成敵人...");
            // SpawnEnemies();  // 你的生成敵人方法
        });
    }

    /// <summary>
    /// 範例 3：倒數結束後啟動多個系統
    /// </summary>
    public void StartComplexGame()
    {
        countdownTimer.StartCountdown(() =>
        {
            Debug.Log("遊戲開始！");

            // 可以在這裡同時啟動多個系統
            // playerController.enabled = true;
            // enemySpawner.StartSpawning();
            // scoreManager.ResetScore();
            // musicManager.PlayGameMusic();
        });
    }

    /// <summary>
    /// 範例 4：檢查是否正在倒數
    /// </summary>
    public void TryPauseGame()
    {
        if (countdownTimer.IsCountingDown)
        {
            Debug.Log("倒數進行中，無法暫停遊戲");
        }
        else
        {
            Debug.Log("暫停遊戲");
            // Time.timeScale = 0;  // 暫停遊戲
        }
    }
}
