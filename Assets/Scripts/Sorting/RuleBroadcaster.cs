using UnityEngine;
using UnityEngine.UI;

public class RuleBroadcaster : MonoBehaviour
{
    [Header("===== Rule Settings =====")]

    [Tooltip("Current sorting rule (initial rule at game start)")]
    [SerializeField] private SortingRule currentRule = SortingRule.ByCategory;

    [Header("===== Switch Mode =====")]

    [Tooltip("How rules switch: Probability (random chance) or FixedInterval (every N trials)")]
    [SerializeField] private SwitchMode switchMode = SwitchMode.Probability;

    [Tooltip("Probability mode: chance of switching per trial (%)")]
    [Range(0, 100)]
    [SerializeField] private int switchProbability = 30;

    [Tooltip("FixedInterval mode: switch every N trials")]
    [SerializeField] private int switchInterval = 5;

    [Header("===== UI =====")]

    [Tooltip("UI Text showing current rule")]
    [SerializeField] private Text ruleUIText;

    public SortingRule CurrentRule => currentRule;
    public bool DidSwitchThisTrial { get; private set; }

    private int trialsSinceLastSwitch;

    private void Start()
    {
        DidSwitchThisTrial = false;
        UpdateUI();
    }

    public void EvaluateSwitch()
    {
        trialsSinceLastSwitch++;
        bool shouldSwitch = false;

        switch (switchMode)
        {
            case SwitchMode.Probability:
                shouldSwitch = Random.Range(0, 100) < switchProbability;
                break;

            case SwitchMode.FixedInterval:
                if (switchInterval > 0 && trialsSinceLastSwitch >= switchInterval)
                    shouldSwitch = true;
                break;
        }

        if (shouldSwitch)
        {
            currentRule = (currentRule == SortingRule.ByCategory)
                ? SortingRule.ByColor
                : SortingRule.ByCategory;

            trialsSinceLastSwitch = 0;
            DidSwitchThisTrial = true;
        }
        else
        {
            DidSwitchThisTrial = false;
        }

        UpdateUI();
    }

    public void ResetState()
    {
        trialsSinceLastSwitch = 0;
        DidSwitchThisTrial = false;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (ruleUIText == null) return;

        switch (currentRule)
        {
            case SortingRule.ByCategory:
                ruleUIText.text = "--- Sort by CATEGORY ---";
                break;
            case SortingRule.ByColor:
                ruleUIText.text = "--- Sort by COLOR ---";
                break;
        }
    }
}
