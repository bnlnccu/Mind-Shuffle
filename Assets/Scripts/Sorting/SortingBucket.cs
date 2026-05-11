using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SortingBucket : MonoBehaviour
{
    [Header("===== Bucket Accept Properties =====")]

    [Tooltip("Category this bucket accepts")]
    [SerializeField] private ItemCategory acceptCategory;

    [Tooltip("Color this bucket accepts")]
    [SerializeField] private ItemColor acceptColor;

    [Header("===== Label Display Mode =====")]

    [Tooltip("Static: show both dimensions / Dynamic: show only current rule dimension")]
    [SerializeField] private LabelMode labelMode = LabelMode.Static;

    [Tooltip("UI Text for bucket label")]
    [SerializeField] private Text bucketLabel;

    [Header("===== Rule Reference =====")]

    [Tooltip("RuleBroadcaster reference")]
    [SerializeField] private RuleBroadcaster ruleBroadcaster;

    [Header("===== Audio Settings =====")]

    [Tooltip("Sound for correct sort")]
    [SerializeField] private AudioClip correctSound;

    [Tooltip("Sound for wrong sort")]
    [SerializeField] private AudioClip wrongSound;

    [Tooltip("AudioSource on this bucket (Play On Awake must be OFF)")]
    [SerializeField] private AudioSource audioPlayer;

    [Header("===== Events =====")]

    [Tooltip("Triggered on correct sort (drag ScoreManager.AddScore here)")]
    [SerializeField] private UnityEvent onCorrectSort;

    [Tooltip("Triggered on wrong sort (drag ScoreManager.SubtractScore here)")]
    [SerializeField] private UnityEvent onWrongSort;

    // ========================================
    // Framework code (students do NOT modify)
    // ========================================

    // Stimulus data broadcast by GameFlowManager (enum values, not object references)
    private ItemCategory currentCategory;
    private ItemColor currentColor;

    // Callback set by GameFlowManager
    private System.Action<bool> onTrialCompleteCallback;

    // Read-only properties for GameFlowManager
    public ItemCategory AcceptCategory => acceptCategory;
    public ItemColor AcceptColor => acceptColor;

    /// <summary>
    /// Called by GameFlowManager when stimulus spawns.
    /// Receives pure enum values, not object references.
    /// </summary>
    public void SetCurrentItem(ItemCategory cat, ItemColor col)
    {
        currentCategory = cat;
        currentColor = col;
    }

    /// <summary>
    /// Called by GameFlowManager to register trial completion callback.
    /// </summary>
    public void SetTrialCompleteCallback(System.Action<bool> callback)
    {
        onTrialCompleteCallback = callback;
    }

    private void Start()
    {
        UpdateLabel();
    }

    /// <summary>
    /// Update bucket label text. Called by GameFlowManager after rule changes.
    /// </summary>
    public void UpdateLabel()
    {
        if (bucketLabel == null || ruleBroadcaster == null) return;

        switch (labelMode)
        {
            case LabelMode.Static:
                bucketLabel.text = ColorToString(acceptColor) + " / " + CategoryToString(acceptCategory);
                break;

            case LabelMode.Dynamic:
                if (ruleBroadcaster.CurrentRule == SortingRule.ByCategory)
                    bucketLabel.text = CategoryToString(acceptCategory);
                else
                    bucketLabel.text = ColorToString(acceptColor);
                break;
        }
    }

    private string CategoryToString(ItemCategory cat)
    {
        switch (cat)
        {
            case ItemCategory.Fruit: return "Fruit";
            case ItemCategory.Animal: return "Animal";
            default: return "---";
        }
    }

    private string ColorToString(ItemColor col)
    {
        switch (col)
        {
            case ItemColor.Red: return "Red";
            case ItemColor.Blue: return "Blue";
            default: return "---";
        }
    }

    // ========================================
    // *** STUDENT FILL-IN-THE-BLANK AREA ***
    // ========================================

    /// <summary>
    /// Try sorting (called by Button OnClick, no parameters).
    /// Checks if the player's sort is correct based on current rule.
    /// </summary>
    public void TrySort()
    {
        if (ruleBroadcaster == null) return;

        SortingRule currentRule = ruleBroadcaster.CurrentRule;
        bool correct = false;

        // ===== TODO (B): 判斷玩家分類是否正確，結果存進 correct =====
        // 如果目前規則是 ByCategory，比較 currentCategory 和 acceptCategory
        // 如果目前規則是 ByColor，比較 currentColor 和 acceptColor
        // 可用的變數: currentRule, currentCategory, currentColor, acceptCategory, acceptColor


        // ===== TODO (C): 根據對錯播放音效 =====
        // 答對播放 correctSound，答錯播放 wrongSound
        // 使用 audioPlayer.PlayOneShot( ) 來播放





        // ===== Framework code below (students do NOT modify) =====
        if (correct)
            onCorrectSort.Invoke();
        else
            onWrongSort.Invoke();

        // Notify GameFlowManager that trial is complete
        onTrialCompleteCallback?.Invoke(correct);
    }
}
