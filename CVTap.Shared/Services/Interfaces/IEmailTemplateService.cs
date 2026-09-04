using CVTap.Shared.Models;

namespace CVTap.Shared.Services.Interfaces;

public interface IEmailTemplateService
{
    Task<IReadOnlyList<EmailTemplate>> GetAllAsync();
    Task<EmailTemplate?> GetByIdAsync(Guid id);
    Task<EmailTemplate?> GetDefaultAsync();
    Task<EmailTemplate> SaveAsync(EmailTemplate template);
    Task<bool> DeleteAsync(Guid id);
    Task SetDefaultAsync(Guid id);
    Task<EmailTemplate> DuplicateAsync(Guid id);
}
