using CVTap.Shared.Models;

namespace CVTap.Shared.Services.Interfaces;

public interface IApplicationHistoryService
{
    Task<IReadOnlyList<ApplicationRecord>> GetAllAsync();
    Task<ApplicationRecord> RecordActionAsync(Guid cvId, string cvTitle, string recipient, string company, string position, ApplicationStatus status, string? notes = null);
    Task UpdateStatusAsync(Guid id, ApplicationStatus status);
    Task<bool> DeleteAsync(Guid id);
}
