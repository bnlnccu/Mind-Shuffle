using UnityEngine;

/// <summary>
/// ScoreManager 使用範例
///
/// 這個腳本示範如何在你的遊戲中使用 ScoreManager 元件。
/// 把這個腳本掛到任意 GameObject 上，然後在 Inspector 中指定 ScoreManager。
///
/// 按鍵測試：
///   - 按 A 鍵：加 10 分
///   - 按 S 鍵：扣 5 分
///   - 按 R 鍵：重置分數
///   - 按 Space：顯示目前分數
/// </summary>
public class ScoreManagerExample : MonoBehaviour
{
    [Header("===== 請在 Inspector 中指定 =====")]
    [Tooltip("拖曳有掛 ScoreManager 元件的物件到這裡")]
    public ScoreManager scoreManager;

    void Update()
    {
        // 範例 1：按 A 加分（例如：收集金幣、擊中目標）
        if (Input.GetKeyDown(KeyCode.A))
        {
            scoreManager.AddScore(10);
            Debug.Log("加 10 分！目前分數：" + scoreManager.CurrentScore);
        }

        // 範例 2：按 S 扣分（例如：被怪物攻擊、犯錯）
        if (Input.GetKeyDown(KeyCode.S))
        {
            scoreManager.SubtractScore(5);
            Debug.Log("扣 5 分！目前分數：" + scoreManager.CurrentScore);
        }

        // 範例 3：按 R 重置分數（例如：遊戲重新開始）
        if (Input.GetKeyDown(KeyCode.R))
        {
            scoreManager.ResetScore();
            Debug.Log("分數已重置為 0");
        }

        // 範例 4：讀取目前分數（可以用來判斷遊戲結束條件）
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int currentScore = scoreManager.CurrentScore;
            Debug.Log("目前分數：" + currentScore);

            // 例如：分數達到 100 就過關
            if (currentScore >= 100)
            {
                Debug.Log("恭喜過關！");
            }
        }
    }

    // ========== 進階範例：在其他腳本中使用 ==========

    /// <summary>
    /// 範例：根據玩家表現給予不同分數
    /// </summary>
    public void OnPlayerHitTarget(bool isPerfectHit)
    {
        if (isPerfectHit)
        {
            scoreManager.AddScore(20);  // 完美擊中：20 分
            Debug.Log("Perfect! +20");
        }
        else
        {
            scoreManager.AddScore(5);   // 普通擊中：5 分
            Debug.Log("Hit! +5");
        }
    }

    /// <summary>
    /// 範例：根據反應時間給予獎勵分數
    /// </summary>
    public void OnReactionCompleted(float reactionTimeMs)
    {
        if (reactionTimeMs < 300f)
        {
            scoreManager.AddScore(15);  // 超快反應：15 分
            Debug.Log("超快反應！+15");
        }
        else if (reactionTimeMs < 500f)
        {
            scoreManager.AddScore(10);  // 快速反應：10 分
            Debug.Log("快速反應！+10");
        }
        else
        {
            scoreManager.AddScore(5);   // 正常反應：5 分
            Debug.Log("反應成功！+5");
        }
    }
}
