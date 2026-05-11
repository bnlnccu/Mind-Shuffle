using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 【可復用元件】反應時間紀錄器：記錄每次反應的時間（毫秒），並提供統計功能。
///
/// 這是心理學實驗的核心工具——用來測量玩家的認知反應速度。
/// 支援兩種使用方式：自動計時（StartTiming/StopTimingMs）或手動記錄（RecordTime）。
///
/// 這個元件可以獨立使用在任何需要測量反應時間的遊戲或實驗中。
/// 例如：反應速度測試、打地鼠、射擊遊戲、記憶遊戲等。
///
/// === 使用方式一：自動計時 ===
///   1. 刺激出現時呼叫 StartTiming()
///   2. 玩家反應時呼叫 StopTimingMs()，會回傳反應時間並自動記錄
///
/// === 使用方式二：手動記錄 ===
///   如果你的遊戲邏輯已經算好反應時間，直接呼叫 RecordTime(ms)
///
/// === 範例程式碼 ===
/// <code>
///   // 自動計時
///   rtRecorder.StartTiming();    // 刺激（地鼠）出現時呼叫
///   float ms = rtRecorder.StopTimingMs();  // 玩家點擊時呼叫，會回傳反應時間
///
///   // 手動記錄
///   float myReactionTime = 523.4f;  // 你自己算出的反應時間
///   rtRecorder.RecordTime(myReactionTime);  // 直接記錄
///
///   // 取得統計資料
///   float avg = rtRecorder.GetAverageMs();  // 平均反應時間
///   List&lt;float&gt; allData = rtRecorder.GetAllRecords();  // 所有紀錄
/// </code>
/// </summary>
public class ReactionTimeRecorder : MonoBehaviour
{
    private List<float> records = new List<float>();
    private float timingStartTime;
    private bool isTiming;

    /// <summary>已記錄的筆數（唯讀）</summary>
    public int RecordCount => records.Count;

    /// <summary>
    /// 開始計時（在刺激出現時呼叫）
    /// 例如：地鼠冒出來、目標出現、聲音播放時
    /// </summary>
    public void StartTiming()
    {
        timingStartTime = Time.time;
        isTiming = true;
    }

    /// <summary>
    /// 停止計時並自動記錄（在玩家反應時呼叫）。
    /// 回傳反應時間（毫秒）。
    /// 例如：玩家點擊滑鼠、按下按鍵時
    /// </summary>
    /// <returns>反應時間（毫秒）。如果沒有先呼叫 StartTiming() 則回傳 0。</returns>
    public float StopTimingMs()
    {
        if (!isTiming) return 0f;

        isTiming = false;
        float ms = (Time.time - timingStartTime) * 1000f;
        records.Add(ms);
        return ms;
    }

    /// <summary>
    /// 直接記錄一筆反應時間（毫秒）。
    /// 適合在其他腳本已經算好反應時間的情況下使用。
    /// </summary>
    /// <param name="ms">反應時間（毫秒）</param>
    public void RecordTime(float ms)
    {
        records.Add(ms);
    }

    /// <summary>
    /// 取得平均反應時間（毫秒）
    /// </summary>
    /// <returns>平均值。如果沒有紀錄則回傳 0。</returns>
    public float GetAverageMs()
    {
        if (records.Count == 0) return 0f;

        float total = 0f;
        foreach (float rt in records)
            total += rt;
        return total / records.Count;
    }

    /// <summary>
    /// 取得所有反應時間紀錄（毫秒）
    /// </summary>
    /// <returns>所有紀錄的副本（List）。可以用來匯出資料或做進階統計。</returns>
    public List<float> GetAllRecords()
    {
        return new List<float>(records);
    }

    /// <summary>
    /// 清除所有紀錄（通常在重新開始遊戲時呼叫）
    /// </summary>
    public void ClearRecords()
    {
        records.Clear();
        isTiming = false;
    }
}
