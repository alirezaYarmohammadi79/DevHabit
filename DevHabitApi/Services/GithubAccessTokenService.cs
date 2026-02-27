using DevHabitApi.Database;
using DevHabitApi.DTOs.Github;
using DevHabitApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevHabitApi.Services;

public sealed class GithubAccessTokenService(ApplicationDbContext context , EncryptionService encryptionService)
{
    public async Task StoreAsync(
        string userId,
        StoreGitHubAccessTokenDto accessTokenDto,
        CancellationToken cancellationToken = default)
    {
        GithubAccessToken? existingAccessToken = await GetAccessTokenAsync(userId , cancellationToken);

        string encryptedToken = encryptionService.Encrypt(accessTokenDto.AccessToken);

        if(existingAccessToken is not null)
        {
            existingAccessToken.Token = encryptedToken;
            existingAccessToken.ExpiresAtUtc = DateTime.UtcNow.AddDays(accessTokenDto.ExpiresInDays);
        }
        else
        {
            context.GithubAccessTokens.Add(new GithubAccessToken
            {
                Id = $"gh_{Guid.CreateVersion7()}",
                UserId = userId,
                Token = encryptedToken,
                CreateAtUtc = DateTime.UtcNow,
                ExpiresAtUtc = DateTime.UtcNow.AddDays(accessTokenDto.ExpiresInDays)
            });
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<string?> GetAsync(string userId , CancellationToken cancellationToken = default)
    {
        GithubAccessToken? githubAccessToken = await GetAccessTokenAsync(userId,  cancellationToken);

        if(githubAccessToken is null)
        {
            return null; 
        }

        string decryptedToken = encryptionService.Decrypt(githubAccessToken.Token);

        return decryptedToken;
    }

    public async Task RevokeAsync(string userId , CancellationToken cancellationToken = default)
    {
        GithubAccessToken? githubAccessToken = await GetAccessTokenAsync(userId, cancellationToken);

        if( githubAccessToken is null)
        {
            return;
        }

        context.GithubAccessTokens.Remove(githubAccessToken);

        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task<GithubAccessToken?> GetAccessTokenAsync(string userId, CancellationToken cancellationToken)
    {
        return await context.GithubAccessTokens.SingleOrDefaultAsync(p=> p.UserId == userId , cancellationToken);
    }
}
