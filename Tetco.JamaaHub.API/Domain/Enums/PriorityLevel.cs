namespace Domain.Enums;

public sealed record PriorityLevel : SmartEnum<PriorityLevel, int>
{
    public PriorityLevel(string key, int value) : base(key, value)
    {
    }

    public static PriorityLevel Heigh = new("heigh", 1);
    public static PriorityLevel Medium = new("medium", 2);
    public static PriorityLevel Low = new("low", 3);
}
