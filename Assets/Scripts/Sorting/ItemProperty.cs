using UnityEngine;

public class ItemProperty : MonoBehaviour
{
    [Header("===== Stimulus Properties =====")]

    [Tooltip("Category of this object (Fruit or Animal)")]
    [SerializeField] private ItemCategory category;

    [Tooltip("Color of this object (Red or Blue)")]
    [SerializeField] private ItemColor color;

    public ItemCategory Category => category;

    public ItemColor Color
    {
        get => color;
        set => color = value;
    }
}
