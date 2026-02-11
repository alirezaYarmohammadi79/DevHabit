namespace DevHabitApi.DTOs.Users;

public sealed record UserDto
{
    public required string Id { get; set; }
    public required string Emial { get; set; }
    public required string Name { get; set; }
    public required DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
}
