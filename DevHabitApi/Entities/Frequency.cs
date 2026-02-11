namespace DevHabitApi.Entities;

public sealed class Frequency
{
    public FrequencyType Type { get; set; }

    public int TimesPerPeriod { get; set; }
}
