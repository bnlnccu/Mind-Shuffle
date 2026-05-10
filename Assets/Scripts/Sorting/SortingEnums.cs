// Assets/Scripts/Sorting/SortingEnums.cs

/// <summary>
/// Shared enum types for Mind-Shuffle sorting game
/// </summary>

// Category of stimuli items
public enum ItemCategory
{
    Fruit,
    Animal
}

// Color of stimuli items
public enum ItemColor
{
    Red,
    Blue
}

// Sorting rule
public enum SortingRule
{
    ByCategory,
    ByColor
}

// Rule switching mode
public enum SwitchMode
{
    Probability,
    FixedInterval
}

// Bucket label display mode
public enum LabelMode
{
    Static,
    Dynamic
}
