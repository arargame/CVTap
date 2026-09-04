using CVTap.Shared.Models;
using CVTap.Shared.Services.Implementations;
using SQLite;

namespace CVTap.Shared.Data;

public class CvTapDatabase
{
    private readonly SQLiteAsyncConnection _database;
    private bool _initialized;
    private readonly SemaphoreSlim _initLock = new(1, 1);

    public CvTapDatabase(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
    }

    public async Task EnsureInitializedAsync()
    {
        if (_initialized) return;

        await _initLock.WaitAsync();
        try
        {
            if (_initialized) return;

            await _database.CreateTableAsync<CvProfile>();
            await _database.CreateTableAsync<EmailTemplate>();
            await _database.CreateTableAsync<UserProfile>();
            await _database.CreateTableAsync<ApplicationRecord>();

            // Seed default templates if none exist
            var templateCount = await _database.Table<EmailTemplate>().CountAsync();
            if (templateCount == 0)
            {
                var defaults = DefaultTemplates.GetDefaults();
                await _database.InsertAllAsync(defaults);
            }

            // Seed empty default profile if none exists
            var profileCount = await _database.Table<UserProfile>().CountAsync();
            if (profileCount == 0)
            {
                var defaultProfile = new UserProfile
                {
                    FullName = "",
                    Email = "",
                    Phone = "",
                    LinkedIn = "",
                    GitHub = "",
                    Website = "",
                    UpdatedAt = DateTime.UtcNow
                };
                await _database.InsertAsync(defaultProfile);
            }

            _initialized = true;
        }
        finally
        {
            _initLock.Release();
        }
    }

    public SQLiteAsyncConnection Connection => _database;
}
