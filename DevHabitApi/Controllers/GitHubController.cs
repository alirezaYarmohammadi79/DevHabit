
using DevHabitApi.DTOs.Github;
using DevHabitApi.Entities;
using DevHabitApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevHabitApi.Controllers;

[Authorize(Roles = Roles.Member)]
[ApiController]
[Route("github")]
public class GitHubController(
    GithubAccessTokenService githubAccessTokenService,
    GitHubService gitHubService,
    UserContext userContext,
    LinkService linkService) : ControllerBase
{
    [HttpPut("personal-access-token")]
    public async Task<IActionResult> StoreAccessToken(StoreGitHubAccessTokenDto storeGitHubAccessTokenDto)
    {
        var userId = await userContext.GetUserIdAsync();

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        await githubAccessTokenService.StoreAsync(userId, storeGitHubAccessTokenDto);

        return NoContent();
    }

    [HttpDelete("personal-access-token")]
    public async Task<IActionResult> RevokeAccessToken()
    {
        var userId = await userContext.GetUserIdAsync();

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        await githubAccessTokenService.RevokeAsync(userId);

        return NoContent();
    }

    [HttpGet("profile")]
    public async Task<ActionResult<GitHubUserProfileDto>> GetUserProfile()
    {
        string? userId = await userContext.GetUserIdAsync();
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        string? accessToken = await githubAccessTokenService.GetAsync(userId);
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return NotFound();
        }

        GitHubUserProfileDto? userProfile = await gitHubService.GetUserProfileAsync(accessToken);

        if (userProfile is null)
        {
            return NotFound();
        }

        userProfile.Links =
        [
            linkService.Create(nameof(GetUserProfile) , "self" , HttpMethods.Get),
            linkService.Create(nameof(StoreAccessToken) , "store-token" , HttpMethods.Put),
            linkService.Create(nameof(RevokeAccessToken) , "revoke-token" , HttpMethods.Delete)
        ];

        return Ok(userProfile);
    }
}
