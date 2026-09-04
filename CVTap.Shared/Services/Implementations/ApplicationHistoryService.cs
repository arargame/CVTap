using CVTap.Shared.Data;
using CVTap.Shared.Models;
using CVTap.Shared.Services.Interfaces;

namespace CVTap.Shared.Services.Implementations;

public class ApplicationHistoryService : IApplicationHistoryService
{
    private readonly CvTapDatabase _db;

    public ApplicationHistoryService(CvTapDatabase db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ApplicationRecord>> GetAllAsync()
    {
        await _db.EnsureInitializedAsync();
        var list = await _db.Connection.Table<ApplicationRecord>().ToListAsync();
        return list.OrderByDescending(x => x.ActionTimestamp).ToList();
    }

    public async Task<ApplicationRecord> RecordActionAsync(
        Guid cvId,
        string cvTitle,
        string recipient,
        string company,
        string position,
        ApplicationStatus status,
        string? notes = null)
    {
        await _db.EnsureInitializedAsync();
        var record = new ApplicationRecord
        {
            Id = Guid.NewGuid(),
            CvProfileId = cvId,
            CvTitle = cvTitle,
            RecipientEmail = recipient,
            Company = company,
            Position = position,
            Status = status,
            ActionTimestamp = DateTime.UtcNow,
            Notes = notes
        };

        await _db.Connection.InsertAsync(record);
        return record;
    }

    public async Task UpdateStatusAsync(Guid id, ApplicationStatus status)
    {
        await _db.EnsureInitializedAsync();
        var record = await _db.Connection.Table<ApplicationRecord>().FirstOrDefaultAsync(r => r.Id == id);
        if (record != null)
        {
            record.Status = status;
            await _db.Connection.UpdateAsync(record);
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        await _db.EnsureInitializedAsync();
        var record = await _db.Connection.Table<ApplicationRecord>().FirstOrDefaultAsync(r => r.Id == id);
        if (record == null) return false;

        await _db.Connection.DeleteAsync(record);
        return true;
    }
}
