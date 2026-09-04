using CVTap.Shared.Models;

namespace CVTap.Shared.Services.Interfaces;

public interface IUserProfileService
{
    Task<UserProfile> GetProfileAsync();
    Task SaveProfileAsync(UserProfile profile);
}
