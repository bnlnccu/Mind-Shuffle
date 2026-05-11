using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ReactionTimeRecorder 使用範例
///
/// 這個腳本示範如何在你的遊戲中使用 ReactionTimeRecorder 元件。
/// 把這個腳本掛到任意 GameObject 上，然後在 Inspector 中指定 ReactionTimeRecorder。
///
/// 按鍵測試：
///   - 按 S 鍵：開始計時（模擬刺激出現）
///   - 按 Space 鍵：停止計時（模擬玩家反應）
///   - 按 A 鍵：顯示平均反應時間
///   - 按 R 鍵：清除所有紀錄
/// </summary>
public class ReactionTimeRecorderExample : MonoBehaviour
{
    [Header("===== 請在 Inspector 中指定 =====")]
    [Tooltip("拖曳有掛 ReactionTimeRecorder 元件的物件到這裡")]
    public ReactionTimeRecorder rtRecorder;

    void Update()
    {
        // 範例 1：按 S 開始計時
        if (Input.GetKeyDown(KeyCode.S))
        {
            rtRecorder.StartTiming();
            Debug.Log("計時開始！（模擬刺激出現）");
        }

        // 範例 2：按 Space 停止計時
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float reactionTime = rtRecorder.StopTimingMs();
            if (reactionTime > 0)
            {
                Debug.Log($"反應時間：{reactionTime:F1} 毫秒");
            }
            else
            {
                Debug.Log("請先按 S 開始計時！");
            }
        }

        // 範例 3：按 A 顯示平均反應時間
        if (Input.GetKeyDown(KeyCode.A))
        {
            ShowStatistics();
        }

        // 範例 4：按 R 清除紀錄
        if (Input.GetKeyDown(KeyCode.R))
        {
            rtRecorder.ClearRecords();
            Debug.Log("所有紀錄已清除");
        }
    }

    // ========== 基本使用範例 ==========

    /// <summary>
    /// 範例 1：打地鼠遊戲中使用自動計時
    /// </summary>
    public void OnMoleAppear()
    {
        // 地鼠出現時開始計時
        rtRecorder.StartTiming();
        Debug.Log("地鼠出現！開始計時...");
    }

    public void OnMoleClicked()
    {
        // 玩家點擊地鼠時停止計時
        float reactionTime = rtRecorder.StopTimingMs();
        Debug.Log($"反應時間：{reactionTime:F1} 毫秒");

        // 根據反應時間給予不同反饋
        if (reactionTime < 300f)
        {
            Debug.Log("超快反應！");
        }
        else if (reactionTime < 500f)
        {
            Debug.Log("快速反應！");
        }
        else
        {
            Debug.Log("反應成功！");
        }
    }

    // ========== 進階使用範例 ==========

    /// <summary>
    /// 範例 2：手動記錄反應時間（如果你自己算好了時間）
    /// </summary>
    public void OnTargetHit(float customReactionTime)
    {
        // 直接記錄一筆資料
        rtRecorder.RecordTime(customReactionTime);
        Debug.Log($"記錄反應時間：{customReactionTime:F1} 毫秒");
    }

    /// <summary>
    /// 範例 3：顯示統計資料
    /// </summary>
    public void ShowStatistics()
    {
        if (rtRecorder.RecordCount == 0)
        {
            Debug.Log("尚無紀錄");
            return;
        }

        float avgReactionTime = rtRecorder.GetAverageMs();
        Debug.Log($"===== 反應時間統計 =====");
        Debug.Log($"總筆數：{rtRecorder.RecordCount}");
        Debug.Log($"平均反應時間：{avgReactionTime:F1} 毫秒");

        // 取得所有紀錄來做進階分析
        List<float> allRecords = rtRecorder.GetAllRecords();

        // 找出最快和最慢的反應時間
        float fastest = float.MaxValue;
        float slowest = 0f;
        foreach (float rt in allRecords)
        {
            if (rt < fastest) fastest = rt;
            if (rt > slowest) slowest = rt;
        }

        Debug.Log($"最快：{fastest:F1} 毫秒");
        Debug.Log($"最慢：{slowest:F1} 毫秒");
    }

    /// <summary>
    /// 範例 4：匯出資料（例如：遊戲結束時）
    /// </summary>
    public void ExportData()
    {
        List<float> allData = rtRecorder.GetAllRecords();

        Debug.Log("===== 反應時間資料 =====");
        for (int i = 0; i < allData.Count; i++)
        {
            Debug.Log($"第 {i + 1} 次：{allData[i]:F1} 毫秒");
        }

        // 實際應用中，你可以把資料存成 CSV 檔案或傳送到伺服器
        // string csv = string.Join(",", allData);
        // System.IO.File.WriteAllText("reaction_times.csv", csv);
    }

    /// <summary>
    /// 範例 5：根據表現給予評價
    /// </summary>
    public void ShowPerformanceRating()
    {
        float avg = rtRecorder.GetAverageMs();

        if (avg == 0)
        {
            Debug.Log("尚無資料");
            return;
        }

        string rating;
        if (avg < 300f)
            rating = "S 級（超快反應！）";
        else if (avg < 400f)
            rating = "A 級（快速反應）";
        else if (avg < 500f)
            rating = "B 級（良好反應）";
        else if (avg < 600f)
            rating = "C 級（普通反應）";
        else
            rating = "D 級（需要多練習）";

        Debug.Log($"你的平均反應時間：{avg:F1} 毫秒");
        Debug.Log($"評價：{rating}");
    }
}
