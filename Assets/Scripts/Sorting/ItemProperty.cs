using UnityEngine;

public class ItemProperty : MonoBehaviour
{
    [Header("===== Stimulus Properties =====")]

    [Tooltip("Category of this object (Fruit or Animal)")]
    [SerializeField] private ItemCategory category;

    public ItemCategory Category => category;

    // Color is assigned at runtime by GameFlowManager (random red/blue)
    // Not shown in Inspector to avoid confusion
    public ItemColor Color { get; set; }
}
