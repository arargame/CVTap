using CVTap.Shared.Data;
using CVTap.Shared.Models;
using CVTap.Shared.Services.Implementations;

namespace CVTap.Services;

public class MobileCvStorageService : BaseCvStorageService
{
    public MobileCvStorageService(CvTapDatabase db) 
        : base(db, FileSystem.AppDataDirectory)
    {
    }

    public override async Task<CvProfile?> PickAndImportPdfAsync(string friendlyName, string? language)
    {
        try
        {
            var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "application/pdf" } },
                    { DevicePlatform.iOS, new[] { "com.adobe.pdf" } },
                    { DevicePlatform.WinUI, new[] { ".pdf" } },
                    { DevicePlatform.MacCatalyst, new[] { "pdf" } },
                });

            var options = new PickOptions
            {
                PickerTitle = "Select your PDF CV",
                FileTypes = customFileType
            };

            var result = await FilePicker.Default.PickAsync(options);
            if (result == null)
            {
                return null;
            }

            using var stream = await result.OpenReadAsync();
            var nameToUse = string.IsNullOrWhiteSpace(friendlyName)
                ? Path.GetFileNameWithoutExtension(result.FileName)
                : friendlyName;

            return await SaveStreamAsync(stream, result.FileName, nameToUse, language);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[MobileCvStorageService] Error picking PDF: {ex.Message}");
            throw;
        }
    }
}
