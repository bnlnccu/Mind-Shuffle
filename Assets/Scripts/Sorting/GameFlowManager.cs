using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameFlowManager : MonoBehaviour
{
    [Header("===== Stimulus Settings =====")]

    [Tooltip("List of all stimulus Prefabs")]
    [SerializeField] private List<GameObject> stimulusPrefabs;

    [Tooltip("Position where stimulus appears")]
    [SerializeField] private Transform spawnPoint;

    [Header("===== Component References =====")]

    [Tooltip("RuleBroadcaster")]
    [SerializeField] private RuleBroadcaster ruleBroadcaster;

    [Tooltip("TrialCounter (GameToolkit)")]
    [SerializeField] private TrialCounter trialCounter;

    [Tooltip("CountdownTimer (GameToolkit)")]
    [SerializeField] private CountdownTimer countdownTimer;

    [Tooltip("ReactionTimeRecorder (GameToolkit)")]
    [SerializeField] private ReactionTimeRecorder reactionTimeRecorder;

    [Tooltip("ScoreManager (GameToolkit)")]
    [SerializeField] private ScoreManager scoreManager;

    [Header("===== Bucket References =====")]

    [Tooltip("Left sorting bucket")]
    [SerializeField] private SortingBucket leftBucket;

    [Tooltip("Right sorting bucket")]
    [SerializeField] private SortingBucket rightBucket;

    [Tooltip("Left bucket Button component")]
    [SerializeField] private Button leftButton;

    [Tooltip("Right bucket Button component")]
    [SerializeField] private Button rightButton;

    [Header("===== Feedback Settings =====")]

    [Tooltip("Feedback icon (O/X), hidden by default")]
    [SerializeField] private GameObject feedbackIcon;

    [Tooltip("Text on feedback icon")]
    [SerializeField] private Text feedbackText;

    [Tooltip("How long feedback icon stays visible (seconds)")]
    [SerializeField] private float feedbackDuration = 0.8f;

    [Header("===== Trial Timer =====")]

    [Tooltip("Time limit per trial in seconds (0 = no limit)")]
    [SerializeField] private float trialTimeLimit = 5f;

    [Tooltip("UI Text showing remaining time (optional)")]
    [SerializeField] private Text timerText;

    [Header("===== Panel Control =====")]

    [Tooltip("Gameplay UI panel (contains all in-game UI, hidden on game end)")]
    [SerializeField] private GameObject gameplayPanel;

    [Tooltip("Result panel (hidden by default, shown on game end)")]
    [SerializeField] private GameObject resultPanel;

    [Tooltip("Accuracy text")]
    [SerializeField] private Text accuracyText;

    [Tooltip("Average RT text")]
    [SerializeField] private Text avgRTText;

    [Tooltip("Maintain trial avg RT text")]
    [SerializeField] private Text maintainRTText;

    [Tooltip("Switch trial avg RT text")]
    [SerializeField] private Text switchRTText;

    [Tooltip("Switch cost text")]
    [SerializeField] private Text switchCostText;

    [Tooltip("Text showing CSV export file path")]
    [SerializeField] private Text exportPathText;

    // ========================================
    // Internal state (students do NOT modify)
    // ========================================

    private bool isProcessing;
    private GameObject currentStimulus;
    private string currentStimulusName;
    private bool lastTrialCorrect;
    private Coroutine trialTimerCoroutine;
    private List<float> maintainRTs = new List<float>();
    private List<float> switchRTs = new List<float>();
    private int correctCount;
    private int totalCount;

    // @DATA log collector (intercepts Debug.Log output from student code)
    private List<string> collectedDataLines = new List<string>();

    // ========================================
    // Game Start
    // ========================================

    private void Start()
    {
        // Register log interceptor to collect @DATA lines from student's Debug.Log
        Application.logMessageReceived += OnLogMessageReceived;

        // CSV header with @DATA prefix for Console filtering
        Debug.Log("@DATA,Trial,Rule,IsSwitch,Stimulus,RT(ms),Correct");

        if (feedbackIcon != null) feedbackIcon.SetActive(false);
        if (resultPanel != null) resultPanel.SetActive(false);

        SetButtonsInteractable(false);

        // Register trial completion callbacks
        leftBucket.SetTrialCompleteCallback(OnTrialComplete);
        rightBucket.SetTrialCompleteCallback(OnTrialComplete);

        countdownTimer.StartCountdown(OnCountdownFinished);
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= OnLogMessageReceived;
    }

    private void OnLogMessageReceived(string logString, string stackTrace, LogType type)
    {
        // Only collect lines that start with @DATA
        if (logString.StartsWith("@DATA"))
            collectedDataLines.Add(logString);
    }

    private void OnCountdownFinished()
    {
        trialCounter.ResetTrials();
        scoreManager.ResetScore();
        reactionTimeRecorder.ClearRecords();
        ruleBroadcaster.ResetState();
        maintainRTs.Clear();
        switchRTs.Clear();
        correctCount = 0;
        totalCount = 0;

        SpawnNextStimulus();
    }

    // ========================================
    // Trial Flow
    // ========================================

    private void SpawnNextStimulus()
    {
        // Update bucket labels (rule may have just switched)
        leftBucket.UpdateLabel();
        rightBucket.UpdateLabel();

        // ===== TODO (A-1): Pick a random Prefab and Instantiate it under spawnPoint =====
        // Store the result in currentStimulus




        // Randomly assign color (Red or Blue) and apply tint
        if (currentStimulus != null)
        {
            ItemColor randomColor = (Random.Range(0, 2) == 0) ? ItemColor.Red : ItemColor.Blue;

            // Set ItemProperty color
            ItemProperty prop = currentStimulus.GetComponent<ItemProperty>();
            if (prop != null)
                prop.Color = randomColor;

            // Apply visual tint to Image
            var image = currentStimulus.GetComponent<UnityEngine.UI.Image>();
            if (image != null)
                image.color = (randomColor == ItemColor.Red)
                    ? new Color(1f, 0.3f, 0.3f, 1f)
                    : new Color(0.3f, 0.5f, 1f, 1f);
        }

        // Cache stimulus name BEFORE it could be destroyed
        currentStimulusName = currentStimulus != null
            ? currentStimulus.name.Replace("(Clone)", "").Trim()
            : "Unknown";

        // Broadcast stimulus data to buckets (enum values, not object reference)
        if (currentStimulus != null)
        {
            ItemProperty prop = currentStimulus.GetComponent<ItemProperty>();
            if (prop != null)
            {
                leftBucket.SetCurrentItem(prop.Category, prop.Color);
                rightBucket.SetCurrentItem(prop.Category, prop.Color);
            }
        }

        // Start timing
        reactionTimeRecorder.StartTiming();

        // Start trial timer (if limit > 0)
        if (trialTimeLimit > 0f)
            trialTimerCoroutine = StartCoroutine(TrialTimerRoutine());

        // Unlock buttons
        isProcessing = false;
        SetButtonsInteractable(true);
    }

    private IEnumerator TrialTimerRoutine()
    {
        float remaining = trialTimeLimit;

        while (remaining > 0f)
        {
            if (timerText != null)
                timerText.text = remaining.ToString("F1") + "s";

            yield return null;
            remaining -= Time.deltaTime;
        }

        if (timerText != null)
            timerText.text = "0.0s";

        // Time's up — auto-fail
        if (!isProcessing)
            OnTrialComplete(false);
    }

    /// <summary>
    /// Called by SortingBucket via callback when trial is complete.
    /// </summary>
    public void OnTrialComplete(bool correct)
    {
        if (isProcessing) return;
        isProcessing = true;
        SetButtonsInteractable(false);

        // Cancel trial timer
        if (trialTimerCoroutine != null)
        {
            StopCoroutine(trialTimerCoroutine);
            trialTimerCoroutine = null;
        }

        lastTrialCorrect = correct;
        if (correct) correctCount++;
        totalCount++;

        // 1. Stop timing, get RT
        float rt = reactionTimeRecorder.StopTimingMs();

        // 2. Destroy stimulus IMMEDIATELY (synchronous, no delay)
        // ===== TODO (A-2): Destroy currentStimulus =====




        // 3. Classify RT (maintain vs switch)
        if (ruleBroadcaster.DidSwitchThisTrial)
            switchRTs.Add(rt);
        else
            maintainRTs.Add(rt);

        // 4. CSV output
        // ===== TODO (D): 用 Debug.Log 印出這回合的資料 =====
        // 格式："@DATA," + 回合數 + "," + 規則 + "," + 是否切換 + "," + 刺激物名稱 + "," + 反應時間 + "," + 是否正確
        // 可用的變數: trialCounter.CurrentTrial, ruleBroadcaster.CurrentRule,
        //            ruleBroadcaster.DidSwitchThisTrial, currentStimulusName, rt, lastTrialCorrect




        // 5. Show feedback icon then continue
        StartCoroutine(ShowFeedbackAndContinue(correct));
    }

    private IEnumerator ShowFeedbackAndContinue(bool correct)
    {
        if (feedbackIcon != null && feedbackText != null)
        {
            feedbackText.text = correct ? "O" : "X";
            feedbackText.color = correct ? Color.green : Color.red;
            feedbackIcon.SetActive(true);
        }

        yield return new WaitForSeconds(feedbackDuration);

        if (feedbackIcon != null) feedbackIcon.SetActive(false);

        trialCounter.NextTrial();
        ruleBroadcaster.EvaluateSwitch();

        if (trialCounter.HasRemaining)
            SpawnNextStimulus();
        else
            ShowResults();
    }

    // ========================================
    // Results
    // ========================================

    private void ShowResults()
    {
        SetButtonsInteractable(false);

        // Hide gameplay UI, show result panel
        if (gameplayPanel != null) gameplayPanel.SetActive(false);
        if (resultPanel == null) return;
        resultPanel.SetActive(true);

        float accuracy = totalCount > 0 ? (float)correctCount / totalCount * 100f : 0f;
        if (accuracyText != null)
            accuracyText.text = "\u7e3d\u6b63\u78ba\u7387\uff1a" + accuracy.ToString("F1") + "%";

        float avgRT = reactionTimeRecorder.GetAverageMs();
        if (avgRTText != null)
            avgRTText.text = "\u5e73\u5747\u53cd\u61c9\u6642\u9593\uff1a" + avgRT.ToString("F1") + " ms";

        float maintainAvg = CalculateAverage(maintainRTs);
        if (maintainRTText != null)
            maintainRTText.text = "\u7dad\u6301\u56de\u5408 RT\uff1a" + maintainAvg.ToString("F1") + " ms";

        float switchAvg = CalculateAverage(switchRTs);
        if (switchRTText != null)
            switchRTText.text = "\u5207\u63db\u56de\u5408 RT\uff1a" + switchAvg.ToString("F1") + " ms";

        float switchCost = switchAvg - maintainAvg;
        if (switchCostText != null)
            switchCostText.text = "\u5207\u63db\u6210\u672c\uff1a" + switchCost.ToString("F1") + " ms";
    }

    // ========================================
    // Result Panel Buttons
    // ========================================

    /// <summary>
    /// Restart the game (called by "Play Again" button).
    /// </summary>
    public void RestartGame()
    {
        if (resultPanel != null) resultPanel.SetActive(false);
        if (gameplayPanel != null) gameplayPanel.SetActive(true);

        // Clear previously collected data
        collectedDataLines.Clear();

        // Re-print CSV header for new session
        Debug.Log("@DATA,Trial,Rule,IsSwitch,Stimulus,RT(ms),Correct");

        SetButtonsInteractable(false);
        countdownTimer.StartCountdown(OnCountdownFinished);
    }

    /// <summary>
    /// Export collected @DATA lines to a CSV file (called by "Export CSV" button).
    /// Only collects lines that student wrote via Debug.Log with @DATA prefix.
    /// If student's format is wrong (no @DATA prefix), the file will be empty.
    /// </summary>
    public void ExportCSV()
    {
        // Remove the @DATA prefix from each line for clean CSV
        var csvLines = new List<string>();
        foreach (var line in collectedDataLines)
        {
            // Strip "@DATA," prefix (6 chars) to get pure CSV
            if (line.Length > 6)
                csvLines.Add(line.Substring(6));
            else
                csvLines.Add(line);
        }

        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = "MindShuffle_" + timestamp + ".csv";
        string filePath = System.IO.Path.Combine(Application.dataPath, "..", fileName);
        string fullPath = System.IO.Path.GetFullPath(filePath);

        System.IO.File.WriteAllLines(fullPath, csvLines.ToArray());
        Debug.Log("CSV exported: " + fullPath + " (" + csvLines.Count + " rows)");

        // Show path on screen so student can find the file
        if (exportPathText != null)
            exportPathText.text = "\u4f5c\u7b54\u7d00\u9304\u5df2\u532f\u51fa\u81f3\uff1a\n" + fullPath;
    }

    /// <summary>
    /// Quit the game (called by "Quit" button).
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ========================================
    // Utilities
    // ========================================

    private void SetButtonsInteractable(bool interactable)
    {
        if (leftButton != null) leftButton.interactable = interactable;
        if (rightButton != null) rightButton.interactable = interactable;
    }

    private float CalculateAverage(List<float> values)
    {
        if (values.Count == 0) return 0f;
        float total = 0f;
        foreach (float v in values) total += v;
        return total / values.Count;
    }
}
