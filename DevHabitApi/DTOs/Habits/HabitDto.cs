using DevHabitApi.DTOs.Common;
using DevHabitApi.Entities;
using Newtonsoft.Json;

namespace DevHabitApi.DTOs.Habits;

public sealed record HabitWithTagsDto : ILinkResponse
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required HabitType Type { get; init; }
    public required FrequencyDto Frequency { get; init; }
    public required TargetDto Target { get; init; }
    public required HabitStatus Status { get; init; }
    public required bool IsArchived { get; init; }
    public DateOnly? EndDate { get; init; }
    public MilestoneDto? Milestone { get; init; }
    public required DateTime CreateAtUtc { get; init; }
    public DateTime? UpdatedAtUtc { get; init; }
    public DateTime? LastCompletedUtc { get; init; }
    public List<LinkDto> Links { get; set; }
    [JsonProperty(Order = int.MaxValue)]
    public required string[] Tags { get; init; }
}

public sealed record HabitWithTagsDtoV2 : ILinkResponse
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required HabitType Type { get; init; }
    public required FrequencyDto Frequency { get; init; }
    public required TargetDto Target { get; init; }
    public required HabitStatus Status { get; init; }
    public required bool IsArchived { get; init; }
    public DateOnly? EndDate { get; init; }
    public MilestoneDto? Milestone { get; init; }
    public required DateTime CreateAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public DateTime? LastCompleted { get; init; }
    public List<LinkDto> Links { get; set; }
    [JsonProperty(Order = int.MaxValue)]
    public required string[] Tags { get; init; }
}

public record HabitDto
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required HabitType Type { get; init; }
    public required FrequencyDto Frequency { get; init; }
    public required TargetDto Target { get; init; }
    public required HabitStatus Status { get; init; }
    public required bool IsArchived { get; init; }
    public DateOnly? EndDate { get; init; }
    public MilestoneDto? Milestone { get; init; }
    public required DateTime CreateAtUtc { get; init; }
    public DateTime? UpdatedAtUtc { get; init; }
    public DateTime? LastCompletedUtc { get; init; }
    public List<LinkDto> Links { get; set; }
}

public sealed record FrequencyDto
{
    public required FrequencyType Type { get; init; }

    public required int TimesPerPeriod { get; init; }
}

public sealed record TargetDto
{
    public required int Value { get; init; }

    public required string Unit { get; init; }
}

public sealed record MilestoneDto
{
    public required int Target { get; init; }

    public required int Current { get; init; }
}
