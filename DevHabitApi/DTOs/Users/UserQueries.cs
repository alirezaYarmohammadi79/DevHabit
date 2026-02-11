using System.Linq.Expressions;
using DevHabitApi.Entities;

namespace DevHabitApi.DTOs.Users;

internal static class UserQueries
{
    public static Expression<Func<User, UserDto>> ProjectToDto()
    {
        return u => new UserDto
        {
            Id = u.Id,
            Name = u.Name,
            CreatedAtUtc = u.CreatedAtUtc,
            Emial = u.Email,
            UpdatedAtUtc = u.UpdatedAtUtc,
        };
    }
}
