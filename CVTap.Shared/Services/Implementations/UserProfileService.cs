using CVTap.Shared.Data;
using CVTap.Shared.Models;
using CVTap.Shared.Services.Interfaces;

namespace CVTap.Shared.Services.Implementations;

public class UserProfileService : IUserProfileService
{
    private readonly CvTapDatabase _db;

    public UserProfileService(CvTapDatabase db)
    {
        _db = db;
    }

    public async Task<UserProfile> GetProfileAsync()
    {
        await _db.EnsureInitializedAsync();
        var profile = await _db.Connection.Table<UserProfile>().FirstOrDefaultAsync();
        if (profile == null)
        {
            profile = new UserProfile
            {
                FullName = "",
                Email = "",
                Phone = "",
                LinkedIn = "",
                GitHub = "",
                Website = "",
                UpdatedAt = DateTime.UtcNow
            };
            await _db.Connection.InsertAsync(profile);
        }
        return profile;
    }

    public async Task SaveProfileAsync(UserProfile profile)
    {
        await _db.EnsureInitializedAsync();
        profile.UpdatedAt = DateTime.UtcNow;

        var existing = await _db.Connection.Table<UserProfile>().FirstOrDefaultAsync();
        if (existing == null)
        {
            await _db.Connection.InsertAsync(profile);
        }
        else
        {
            profile.Id = existing.Id;
            await _db.Connection.UpdateAsync(profile);
        }
    }
}
