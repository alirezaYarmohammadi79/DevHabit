using DevHabitApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DevHabitApi.DTOs.Habits;

public sealed record HabitsQueryParameters
{
    [FromQuery(Name = "q")]
    public string? Search { get; set; }

    public HabitStatus? Status { get; init; }

    public HabitType? Type { get; init; }

    public string? Sort { get; init; }
}
