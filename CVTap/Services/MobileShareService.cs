using CVTap.Shared.Services.Interfaces;
#if ANDROID
using Android.Content;
using AndroidX.Core.Content;
using Application = Android.App.Application;
#endif

namespace CVTap.Services;

public class MobileShareService : IShareService
{
    public async Task<bool> ShareFileAsync(string title, string physicalFilePath, string? textContent = null)
    {
        if (string.IsNullOrEmpty(physicalFilePath) || !File.Exists(physicalFilePath))
        {
            return false;
        }

#if ANDROID
        try
        {
            var context = Platform.CurrentActivity ?? Application.Context;
            var file = new Java.IO.File(physicalFilePath);
            var authority = $"{context.PackageName}.fileprovider";
            var uri = AndroidX.Core.Content.FileProvider.GetUriForFile(context, authority, file);

            var intent = new Intent(Intent.ActionSend);
            intent.SetType("application/pdf");
            intent.PutExtra(Intent.ExtraStream, uri);

            if (!string.IsNullOrWhiteSpace(textContent))
            {
                intent.PutExtra(Intent.ExtraText, textContent);
            }

            intent.AddFlags(ActivityFlags.GrantReadUriPermission);

            var chooser = Intent.CreateChooser(intent, title);
            chooser.AddFlags(ActivityFlags.NewTask | ActivityFlags.GrantReadUriPermission);
            context.StartActivity(chooser);
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[MobileShareService] Android intent share failed: {ex.Message}. Trying MAUI Share API.");
        }
#endif

        try
        {
            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = title,
                File = new ShareFile(physicalFilePath)
            });
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[MobileShareService] Share API failed: {ex.Message}");

#if !ANDROID
            // On Windows desktop, fallback to showing file in Windows File Explorer
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{physicalFilePath}\"");
                return true;
            }
            catch
            {
                return false;
            }
#else
            return false;
#endif
        }
    }
}
