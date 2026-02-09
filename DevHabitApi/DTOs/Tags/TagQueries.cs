using System.Linq.Expressions;
using DevHabitApi.DTOs.Tags;
using DevHabitApi.Entities;

namespace DevHabitApi.DTOs.Habits;

internal static class TagQueries
{
    public static Expression<Func<Tag, TagDto>> ProjectToDto()
    {
        return t => new TagDto
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            CreatedAtUtc = t.CreatedAtUtc,
            UpdatedAtUtc = t.UpdatedAtUtc
        };
    }
}
