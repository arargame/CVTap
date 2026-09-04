using CVTap.Shared.Models;

namespace CVTap.Shared.Services.Interfaces;

public interface ICvStorageService
{
    Task<IReadOnlyList<CvProfile>> GetAllAsync();
    Task<CvProfile?> GetByIdAsync(Guid id);
    Task<CvProfile?> GetDefaultAsync();
    Task<CvProfile> SaveStreamAsync(Stream sourceStream, string fileName, string friendlyName, string? language);
    Task<CvProfile?> PickAndImportPdfAsync(string friendlyName, string? language);
    Task<bool> DeleteAsync(Guid id);
    Task SetDefaultAsync(Guid id);
    string GetPhysicalPath(CvProfile cv);
}
