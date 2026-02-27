using DevHabitApi.DTOs.Common;

namespace DevHabitApi.DTOs.Github;

public sealed record GitHubUserProfileDto
{
    public string login { get; set; }
    public string name { get; set; }
    public string AvatarUrl { get; set; }
    public string bio { get; set; }
    public int public_repos { get; set; }
    public int followers { get; set; }
    public int following { get; set; }
    public List<LinkDto> Links { get; set; }
}
