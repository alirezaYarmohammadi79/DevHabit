namespace DevHabitApi.Entities;

public sealed class Habit
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string UserId { get; set; }
    public string? Description { get; set; }
    public HabitType Type { get; set; }
    public Frequency Frequency { get; set; }
    public Target Target { get; set; }
    public HabitStatus Status { get; set; }
    public bool IsArchived { get; set; }
    public DateOnly? EndDate { get; set; }
    public Milestone? Milestone { get; set; }
    public DateTime CreateAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public DateTime? LastCompletedUtc { get; set; }
    public List<HabitTag> HabitTags { get; set; }
    public List<Tag> Tags { get; set; }
}
