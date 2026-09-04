using CVTap.Shared.Data;
using CVTap.Shared.Models;
using CVTap.Shared.Services.Interfaces;

namespace CVTap.Shared.Services.Implementations;

public class BaseCvStorageService : ICvStorageService
{
    protected readonly CvTapDatabase Db;
    protected readonly string BaseStorageDirectory;

    public BaseCvStorageService(CvTapDatabase db, string baseStorageDirectory)
    {
        Db = db;
        BaseStorageDirectory = baseStorageDirectory;

        // Ensure root directories exist
        var cvsDir = Path.Combine(BaseStorageDirectory, "cvs");
        if (!Directory.Exists(cvsDir))
        {
            Directory.CreateDirectory(cvsDir);
        }
    }

    public virtual string GetPhysicalPath(CvProfile cv)
    {
        if (Path.IsPathRooted(cv.LocalPath))
        {
            return cv.LocalPath;
        }
        return Path.Combine(BaseStorageDirectory, cv.LocalPath);
    }

    public virtual async Task<IReadOnlyList<CvProfile>> GetAllAsync()
    {
        await Db.EnsureInitializedAsync();
        var list = await Db.Connection.Table<CvProfile>().ToListAsync();
        return list.OrderByDescending(c => c.IsDefault).ThenBy(c => c.Name).ToList();
    }

    public virtual async Task<CvProfile?> GetByIdAsync(Guid id)
    {
        await Db.EnsureInitializedAsync();
        return await Db.Connection.Table<CvProfile>().FirstOrDefaultAsync(c => c.Id == id);
    }

    public virtual async Task<CvProfile?> GetDefaultAsync()
    {
        await Db.EnsureInitializedAsync();
        var def = await Db.Connection.Table<CvProfile>().FirstOrDefaultAsync(c => c.IsDefault);
        if (def != null) return def;

        return await Db.Connection.Table<CvProfile>().FirstOrDefaultAsync();
    }

    public virtual async Task<CvProfile> SaveStreamAsync(
        Stream sourceStream,
        string fileName,
        string friendlyName,
        string? language)
    {
        await Db.EnsureInitializedAsync();

        var id = Guid.NewGuid();
        var targetFolder = Path.Combine(BaseStorageDirectory, "cvs", id.ToString());
        Directory.CreateDirectory(targetFolder);

        // Sanitize file name
        var safeFileName = Path.GetFileName(fileName);
        if (string.IsNullOrWhiteSpace(safeFileName))
        {
            safeFileName = "cv.pdf";
        }
        if (!safeFileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
        {
            safeFileName += ".pdf";
        }

        var fullFilePath = Path.Combine(targetFolder, safeFileName);
        using (var fileStream = new FileStream(fullFilePath, FileMode.Create, FileAccess.Write))
        {
            await sourceStream.CopyToAsync(fileStream);
        }

        var fileInfo = new FileInfo(fullFilePath);

        // Check if there are any existing CVs
        var existingCount = await Db.Connection.Table<CvProfile>().CountAsync();

        var profile = new CvProfile
        {
            Id = id,
            Name = string.IsNullOrWhiteSpace(friendlyName) ? Path.GetFileNameWithoutExtension(safeFileName) : friendlyName,
            FileName = safeFileName,
            LocalPath = Path.Combine("cvs", id.ToString(), safeFileName),
            Language = string.IsNullOrWhiteSpace(language) ? "EN" : language.ToUpperInvariant(),
            FileSizeBytes = fileInfo.Length,
            CreatedAt = DateTime.UtcNow,
            IsDefault = (existingCount == 0) // First CV is default automatically
        };

        await Db.Connection.InsertAsync(profile);
        return profile;
    }

    public virtual Task<CvProfile?> PickAndImportPdfAsync(string friendlyName, string? language)
    {
        // Default base implementation returns null. Platform-specific overrides will handle picking.
        return Task.FromResult<CvProfile?>(null);
    }

    public virtual async Task<bool> DeleteAsync(Guid id)
    {
        await Db.EnsureInitializedAsync();
        var existing = await Db.Connection.Table<CvProfile>().FirstOrDefaultAsync(c => c.Id == id);
        if (existing == null) return false;

        // Delete physical folder and file
        try
        {
            var folder = Path.Combine(BaseStorageDirectory, "cvs", id.ToString());
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }
        }
        catch
        {
            // Ignore IO deletion errors if file was already moved or deleted
        }

        await Db.Connection.DeleteAsync(existing);

        // If the deleted one was default, set another one as default
        if (existing.IsDefault)
        {
            var next = await Db.Connection.Table<CvProfile>().FirstOrDefaultAsync();
            if (next != null)
            {
                next.IsDefault = true;
                await Db.Connection.UpdateAsync(next);
            }
        }

        return true;
    }

    public virtual async Task SetDefaultAsync(Guid id)
    {
        await Db.EnsureInitializedAsync();
        var all = await Db.Connection.Table<CvProfile>().ToListAsync();
        foreach (var item in all)
        {
            bool shouldBeDefault = (item.Id == id);
            if (item.IsDefault != shouldBeDefault)
            {
                item.IsDefault = shouldBeDefault;
                await Db.Connection.UpdateAsync(item);
            }
        }
    }
}
