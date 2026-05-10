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

    [Header("===== Result Panel =====")]

    [Tooltip("Result panel (hidden by default)")]
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

    // ========================================
    // Internal state (students do NOT modify)
    // ========================================

    private bool isProcessing;
    private GameObject currentStimulus;
    private string currentStimulusName;
    private bool lastTrialCorrect;
    private List<float> maintainRTs = new List<float>();
    private List<float> switchRTs = new List<float>();
    private int correctCount;
    private int totalCount;

    // ========================================
    // Game Start
    // ========================================

    private void Start()
    {
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

        // ===== TODO (Fill-in-the-blank A - Spawn): Instantiate a random stimulus =====
        // Hint:
        //   int randomIndex = Random.Range(0, stimulusPrefabs.Count);
        //   currentStimulus = Instantiate(stimulusPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);



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

        // Unlock buttons
        isProcessing = false;
        SetButtonsInteractable(true);
    }

    /// <summary>
    /// Called by SortingBucket via callback when trial is complete.
    /// </summary>
    public void OnTrialComplete(bool correct)
    {
        if (isProcessing) return;
        isProcessing = true;
        SetButtonsInteractable(false);

        lastTrialCorrect = correct;
        if (correct) correctCount++;
        totalCount++;

        // 1. Stop timing, get RT
        float rt = reactionTimeRecorder.StopTimingMs();

        // 2. Destroy stimulus IMMEDIATELY (synchronous, no delay)
        // ===== TODO (Fill-in-the-blank A - Destroy): Destroy the current stimulus =====
        // Hint: Destroy(currentStimulus);



        // 3. Classify RT (maintain vs switch)
        if (ruleBroadcaster.DidSwitchThisTrial)
            switchRTs.Add(rt);
        else
            maintainRTs.Add(rt);

        // 4. CSV output
        // ===== TODO (Fill-in-the-blank D): Output CSV raw data via Debug.Log =====
        // Hint: Debug.Log("@DATA," + trialCounter.CurrentTrial + "," + ... );
        //   Column order: Trial, Rule, IsSwitch, Stimulus, RT(ms), Correct
        //   Use currentStimulusName (NOT currentStimulus.name -- object is already destroyed!)



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

        if (resultPanel == null) return;
        resultPanel.SetActive(true);

        float accuracy = totalCount > 0 ? (float)correctCount / totalCount * 100f : 0f;
        if (accuracyText != null)
            accuracyText.text = accuracy.ToString("F1") + "%";

        float avgRT = reactionTimeRecorder.GetAverageMs();
        if (avgRTText != null)
            avgRTText.text = avgRT.ToString("F1") + " ms";

        float maintainAvg = CalculateAverage(maintainRTs);
        if (maintainRTText != null)
            maintainRTText.text = maintainAvg.ToString("F1") + " ms";

        float switchAvg = CalculateAverage(switchRTs);
        if (switchRTText != null)
            switchRTText.text = switchAvg.ToString("F1") + " ms";

        float switchCost = switchAvg - maintainAvg;
        if (switchCostText != null)
            switchCostText.text = switchCost.ToString("F1") + " ms";
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
