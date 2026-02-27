namespace DevHabitApi.DTOs.Github;

public sealed record StoreGitHubAccessTokenDto
{
    public required string AccessToken { get; init; }
    public required double ExpiresInDays { get; init; }
}
