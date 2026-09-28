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
            // 1. Ensure required runtime storage permissions on Android / platforms where applicable
            await EnsureStoragePermissionAsync();

            // 2. Define multi-platform file types with broad Android MIME support
            // Android often classifies downloaded PDFs as application/x-pdf or application/vnd.pdf
            var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    {
                        DevicePlatform.Android,
                        new[]
                        {
                            "application/pdf",
                            "application/x-pdf",
                            "application/vnd.pdf",
                            "text/pdf"
                        }
                    },
                    { DevicePlatform.iOS, new[] { "com.adobe.pdf", "public.pdf" } },
                    { DevicePlatform.WinUI, new[] { ".pdf" } },
                    { DevicePlatform.MacCatalyst, new[] { "pdf", "com.adobe.pdf" } },
                });

            var options = new PickOptions
            {
                PickerTitle = "Select your PDF CV",
                FileTypes = customFileType
            };

            FileResult? result = null;
            try
            {
                result = await FilePicker.Default.PickAsync(options);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MobileCvStorageService] Strict MIME file picker failed: {ex.Message}. Falling back to default file picker.");
                // Fallback to general picker if device OEM file manager fails with MIME filters
                result = await FilePicker.Default.PickAsync();
            }

            if (result == null)
            {
                return null;
            }

            // Ensure selected file is indeed a PDF
            if (!result.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Selected file is not a PDF. Please select a valid .pdf file.");
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

    private static async Task EnsureStoragePermissionAsync()
    {
        try
        {
            // Check & request storage permission (crucial on Android <= 12)
            var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageRead>();
            }

            if (status != PermissionStatus.Granted)
            {
                System.Diagnostics.Debug.WriteLine("[MobileCvStorageService] Storage permission not granted by user.");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[MobileCvStorageService] Storage permission request note: {ex.Message}");
        }
    }
}
