using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

/// <summary>
/// 【可復用元件】倒數計時器：在畫面中央顯示 3、2、1 倒數，結束後通知遊戲開始。
///
/// 這個元件可以獨立使用在任何需要開場倒數的遊戲中。
/// 支援自訂倒數數字、速度、以及縮放動畫效果。
///
/// 使用方式：
///   1. 將此元件掛到任意 GameObject 上
///   2. 在 Inspector 中指定顯示倒數的 Text 元件
///   3. 調整倒數參數（從幾開始倒、每個數字停留多久）
///   4. 在程式碼中呼叫 StartCountdown() 並傳入倒數結束後要執行的動作
///
/// 範例：
///   countdownTimer.StartCountdown(() => {
///       Debug.Log("倒數結束，遊戲開始！");
///       // 在這裡寫遊戲開始的邏輯
///   });
/// </summary>
public class CountdownTimer : MonoBehaviour
{
    [Header("===== 倒數設定 =====")]

    [Tooltip("顯示倒數數字的 Text 元件（可選）。如果留空，會靜默倒數（不顯示畫面）。")]
    [SerializeField] private Text countdownText;

    [Tooltip("從幾開始倒數（例如：3 表示 3、2、1）")]
    [Range(1, 10)]
    [SerializeField] private int countFrom = 3;

    [Tooltip("每個數字停留幾秒")]
    [Range(0.5f, 3f)]
    [SerializeField] private float secondsPerCount = 1f;

    [Tooltip("是否使用縮放動畫（數字從大到小的效果）")]
    [SerializeField] private bool useScaleAnimation = true;

    /// <summary>是否正在倒數中（唯讀）</summary>
    public bool IsCountingDown { get; private set; }

    private Coroutine countdownCoroutine;

    /// <summary>
    /// 開始倒數。倒數結束後會自動呼叫 onFinished。
    /// </summary>
    /// <param name="onFinished">倒數結束時要執行的動作（回呼函數）</param>
    public void StartCountdown(Action onFinished)
    {
        StopCountdown();
        countdownCoroutine = StartCoroutine(CountdownRoutine(onFinished));
    }

    /// <summary>中斷倒數（通常不需要手動呼叫）</summary>
    public void StopCountdown()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
        IsCountingDown = false;

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);
    }

    private IEnumerator CountdownRoutine(Action onFinished)
    {
        IsCountingDown = true;

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);

            for (int i = countFrom; i >= 1; i--)
            {
                countdownText.text = i.ToString();

                if (useScaleAnimation)
                {
                    // 從 1.5 倍縮小到 1 倍的動畫
                    countdownText.transform.localScale = Vector3.one * 1.5f;
                    float timer = 0f;
                    while (timer < secondsPerCount)
                    {
                        timer += Time.deltaTime;
                        float t = Mathf.Clamp01(timer / secondsPerCount);
                        countdownText.transform.localScale = Vector3.Lerp(
                            Vector3.one * 1.5f, Vector3.one, t);
                        yield return null;
                    }
                }
                else
                {
                    yield return new WaitForSeconds(secondsPerCount);
                }
            }

            countdownText.gameObject.SetActive(false);
        }
        else
        {
            // 沒有 Text 元件時，單純等待（靜默倒數）
            yield return new WaitForSeconds(countFrom * secondsPerCount);
        }

        IsCountingDown = false;
        countdownCoroutine = null;
        onFinished?.Invoke();
    }
}
