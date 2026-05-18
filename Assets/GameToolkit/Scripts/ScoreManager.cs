using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 【可復用元件】計分管理器：負責加分、扣分、顯示分數。
///
/// 這個元件可以獨立使用在任何需要計分的遊戲中。
/// 學生可直接呼叫 AddScore() / SubtractScore() 來操作分數，
/// 元件會自動更新 UI 顯示。
///
/// 使用方式：
///   1. 將此元件掛到任意 GameObject 上
///   2. 在 Inspector 中指定顯示分數的 Text 元件
///   3. 在遊戲邏輯中呼叫 AddScore() 或 SubtractScore()
///
/// 範例：
///   scoreManager.AddScore(10);        // 加 10 分
///   scoreManager.SubtractScore(5);    // 扣 5 分（不會低於 0）
///   int score = scoreManager.CurrentScore;  // 讀取目前分數
/// </summary>
public class ScoreManager : MonoBehaviour
{
    [Header("===== 計分設定 =====")]

    [Tooltip("顯示分數的 Text 元件（可選）。如果不需要顯示，可以留空。")]
    [SerializeField] private Text scoreText;

    [Tooltip("分數顯示格式，{0} 會被替換成目前分數。例如：「分數：{0}」或「Score: {0}」")]
    [SerializeField] private string displayFormat = "分數：{0}";

    /// <summary>目前的分數（唯讀）</summary>
    public int CurrentScore { get; private set; }

    /// <summary>
    /// 加分
    /// </summary>
    /// <param name="points">要增加的分數</param>
    public void AddScore(int points)
    {
        // ===== TODO (S-1): 把 points 加到 CurrentScore，然後呼叫 UpdateUI() =====
        // CurrentScore 是目前的分數，points 是要加的分數

    }

    /// <summary>
    /// 扣分（分數不會低於 0）
    /// </summary>
    /// <param name="points">要扣除的分數</param>
    public void SubtractScore(int points)
    {
        // ===== TODO (S-2): 從 CurrentScore 扣掉 points，分數不能低於 0，然後呼叫 UpdateUI() =====
        // 提示：用 Mathf.Max(0, ...) 確保分數不會變成負數

    }

    /// <summary>重置分數為 0</summary>
    public void ResetScore()
    {
        CurrentScore = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = string.Format(displayFormat, CurrentScore);
    }
}
