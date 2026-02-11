using DevHabitApi.Entities;

namespace DevHabitApi.DTOs.Habits;

internal static class HabitQueries
{
    public static System.Linq.Expressions.Expression<Func<Habit, HabitDto>> ProjectToDto()
    {
        return h => new HabitDto
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

    public static System.Linq.Expressions.Expression<Func<Habit, HabitWithTagsDto>> ProjectToDtoWithTags()
    {
        return h => new HabitWithTagsDto
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
            Tags = h.Tags.Select(t=> t.Name).ToArray()
        };
    }

    public static System.Linq.Expressions.Expression<Func<Habit, HabitWithTagsDtoV2>> ProjectToDtoWithTagsV2()
    {
        return h => new HabitWithTagsDtoV2
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
            CreateAt = h.CreateAtUtc,
            UpdatedAt = h.UpdatedAtUtc,
            LastCompleted = h.LastCompletedUtc,
            Tags = h.Tags.Select(t => t.Name).ToArray()
        };
    }

}
