using CVTap.Shared.Data;
using CVTap.Shared.Models;
using CVTap.Shared.Services.Interfaces;

namespace CVTap.Shared.Services.Implementations;

public class EmailTemplateService : IEmailTemplateService
{
    private readonly CvTapDatabase _db;

    public EmailTemplateService(CvTapDatabase db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<EmailTemplate>> GetAllAsync()
    {
        await _db.EnsureInitializedAsync();
        var list = await _db.Connection.Table<EmailTemplate>().ToListAsync();
        return list.OrderByDescending(t => t.IsDefault).ThenBy(t => t.Title).ToList();
    }

    public async Task<EmailTemplate?> GetByIdAsync(Guid id)
    {
        await _db.EnsureInitializedAsync();
        return await _db.Connection.Table<EmailTemplate>().FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<EmailTemplate?> GetDefaultAsync()
    {
        await _db.EnsureInitializedAsync();
        var defaultTemplate = await _db.Connection.Table<EmailTemplate>().FirstOrDefaultAsync(t => t.IsDefault);
        if (defaultTemplate != null) return defaultTemplate;

        // Fallback to first available template
        return await _db.Connection.Table<EmailTemplate>().FirstOrDefaultAsync();
    }

    public async Task<EmailTemplate> SaveAsync(EmailTemplate template)
    {
        await _db.EnsureInitializedAsync();
        template.UpdatedAt = DateTime.UtcNow;

        if (template.IsDefault)
        {
            // Clear other defaults
            var all = await _db.Connection.Table<EmailTemplate>().ToListAsync();
            foreach (var item in all.Where(x => x.Id != template.Id && x.IsDefault))
            {
                item.IsDefault = false;
                await _db.Connection.UpdateAsync(item);
            }
        }

        var existing = await _db.Connection.Table<EmailTemplate>().FirstOrDefaultAsync(t => t.Id == template.Id);
        if (existing == null)
        {
            await _db.Connection.InsertAsync(template);
        }
        else
        {
            await _db.Connection.UpdateAsync(template);
        }

        return template;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        await _db.EnsureInitializedAsync();
        var existing = await _db.Connection.Table<EmailTemplate>().FirstOrDefaultAsync(t => t.Id == id);
        if (existing == null) return false;

        await _db.Connection.DeleteAsync(existing);

        // If the deleted one was default, set another one as default
        if (existing.IsDefault)
        {
            var next = await _db.Connection.Table<EmailTemplate>().FirstOrDefaultAsync();
            if (next != null)
            {
                next.IsDefault = true;
                await _db.Connection.UpdateAsync(next);
            }
        }

        return true;
    }

    public async Task SetDefaultAsync(Guid id)
    {
        await _db.EnsureInitializedAsync();
        var all = await _db.Connection.Table<EmailTemplate>().ToListAsync();
        foreach (var item in all)
        {
            bool shouldBeDefault = (item.Id == id);
            if (item.IsDefault != shouldBeDefault)
            {
                item.IsDefault = shouldBeDefault;
                await _db.Connection.UpdateAsync(item);
            }
        }
    }

    public async Task<EmailTemplate> DuplicateAsync(Guid id)
    {
        await _db.EnsureInitializedAsync();
        var original = await GetByIdAsync(id);
        if (original == null)
        {
            throw new InvalidOperationException($"Template with ID {id} not found.");
        }

        var duplicate = new EmailTemplate
        {
            Id = Guid.NewGuid(),
            Title = $"{original.Title} (Copy)",
            Language = original.Language,
            SubjectTemplate = original.SubjectTemplate,
            BodyTemplate = original.BodyTemplate,
            IsDefault = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _db.Connection.InsertAsync(duplicate);
        return duplicate;
    }
}
