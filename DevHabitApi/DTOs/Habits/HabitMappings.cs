using DevHabitApi.Entities;

namespace DevHabitApi.DTOs.Habits;

internal static class HabitMappings
{
    public static HabitDto ToDto(this Habit h)
    {
        return new HabitDto
        {
            Id = h.Id,
            Name = h.Name,
            Description = h.Description,
            Type = h.Type,
            Frequency = new FrequencyDto
            {
                Type = h.Frequency.Type,
                TimesPerPeriod = h.Frequency.TimesPerPeriod
            },
            Target = new TargetDto
            {
                Value = h.Target.Value,
                Unit = h.Target.Unit,
            },
            Status = h.Status,
            IsArchived = h.IsArchived,
            EndDate = h.EndDate,
            Milestone = h.Milestone == null ? null : new MilestoneDto
            {
                Target = h.Milestone.Target,
                Current = h.Milestone.Current
            },
            CreateAtUtc = h.CreateAtUtc,
            UpdatedAtUtc = h.UpdatedAtUtc,
            LastCompletedUtc = h.LastCompletedUtc,
        };
    }

    public static Habit ToEntity(this CreateHabitDto dto)
    {
        Habit habit = new()
        {
            Id = $"h_{Guid.CreateVersion7()}",
            Name = dto.Name,
            Description = dto.Description,
            Type = dto.Type,
            Frequency = new Frequency
            {
                Type = dto.Frequency.Type,
                TimesPerPeriod = dto.Frequency.TimesPerPeriod
            },
            Target = new Target
            {
                Value = dto.Target.Value,
                Unit = dto.Target.Unit,
            },
            Status = HabitStatus.Ongoing,
            IsArchived = false,
            EndDate = dto.EndDate,
            Milestone = dto.Milestone == null ? null : new Milestone
            {
                Target = dto.Milestone.Target,
                Current = dto.Milestone.Current
            },
            CreateAtUtc = DateTime.UtcNow
        };

        return habit;
    }

    public static void UpdateFromDto(this Habit habit, UpdateHabitDto dto)
    {
        // update basic properties
        habit.Name = dto.Name;
        habit.Description = dto.Description;
        habit.Type = dto.Type;
        habit.EndDate = dto.EndDate;

        //update frequency 
        habit.Frequency = new Frequency
        {
            Type = dto.Frequency.Type,
            TimesPerPeriod = dto.Frequency.TimesPerPeriod
        };

        //update target
        habit.Target = new Target
        {
            Value = dto.Target.Value,
            Unit = dto.Target.Unit
        };

        //update milestone
        if (dto.Milestone != null)
        {
            habit.Milestone ??= new Milestone();
            habit.Milestone.Target = dto.Milestone.Target;
        }

        habit.UpdatedAtUtc = DateTime.UtcNow;
    }
}
