using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 【可復用元件】試驗計數器：追蹤目前第幾回合、總共幾回合。
///
/// 這個元件可以獨立使用在任何需要計數回合的遊戲或實驗中。
/// 適合用在需要固定次數試驗的心理學實驗、關卡計數、波次計數等。
///
/// 使用方式：
///   1. 將此元件掛到任意 GameObject 上
///   2. 在 Inspector 中指定顯示進度的 Text 元件
///   3. 設定總回合數（totalTrials）
///   4. 每完成一回合呼叫 NextTrial()
///   5. 用 HasRemaining 判斷是否還有剩餘回合
///
/// 範例：
///   trialCounter.NextTrial();  // 推進到下一回合
///   if (!trialCounter.HasRemaining) {
///       Debug.Log("所有回合已完成！");
///   }
/// </summary>
public class TrialCounter : MonoBehaviour
{
    [Header("===== 試驗設定 =====")]

    [Tooltip("顯示試驗進度的 Text 元件（可選）。如果不需要顯示，可以留空。")]
    [SerializeField] private Text trialText;

    [Tooltip("總共要進行幾次試驗（可在程式碼中修改）")]
    [Range(5, 200)]
    [SerializeField] private int totalTrials = 30;

    [Tooltip("顯示格式，{0}=目前回合，{1}=總回合。例如：「第 {0} / {1} 回」")]
    [SerializeField] private string displayFormat = "第 {0} / {1} 隻";

    /// <summary>目前第幾回合（唯讀，從 0 開始計數）</summary>
    public int CurrentTrial { get; private set; }

    /// <summary>
    /// 總回合數（可在遊戲開始前動態修改）
    /// </summary>
    public int TotalTrials
    {
        get => totalTrials;
        set => totalTrials = value;
    }

    /// <summary>是否還有剩餘回合（唯讀）</summary>
    public bool HasRemaining => CurrentTrial < totalTrials;

    /// <summary>
    /// 推進到下一回合（通常在完成一次試驗後呼叫）
    /// </summary>
    public void NextTrial()
    {
        CurrentTrial++;
        UpdateUI();
    }

    /// <summary>重置為第 0 回合（通常在遊戲重新開始時呼叫）</summary>
    public void ResetTrials()
    {
        CurrentTrial = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (trialText != null)
            trialText.text = string.Format(displayFormat, CurrentTrial, totalTrials);
    }
}
