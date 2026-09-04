using CVTap.Shared.Services.Interfaces;
#if ANDROID
using Android.Content;
using AndroidX.Core.Content;
using Application = Android.App.Application;
#endif

namespace CVTap.Services;

public class MobileEmailComposerService : IEmailComposerService
{
    public Task<bool> CanComposeEmailAsync()
    {
        try
        {
            return Task.FromResult(Email.Default.IsComposeSupported);
        }
        catch
        {
            return Task.FromResult(true);
        }
    }

    public async Task<bool> ComposeEmailAsync(string recipient, string subject, string body, string? attachmentPhysicalPath = null)
    {
        try
        {
            var message = new EmailMessage
            {
                Subject = subject ?? string.Empty,
                Body = body ?? string.Empty,
                To = string.IsNullOrWhiteSpace(recipient) 
                    ? new List<string>() 
                    : new List<string> { recipient.Trim() }
            };

            if (!string.IsNullOrEmpty(attachmentPhysicalPath) && File.Exists(attachmentPhysicalPath))
            {
                message.Attachments.Add(new EmailAttachment(attachmentPhysicalPath));
            }

            if (Email.Default.IsComposeSupported)
            {
                await Email.Default.ComposeAsync(message);
                return true;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[MobileEmailComposer] ComposeAsync failed: {ex.Message}. Falling back to platform intent/shell.");
        }

#if ANDROID
        try
        {
            var context = Platform.CurrentActivity ?? Application.Context;
            var intent = new Intent(Intent.ActionSend);
            intent.SetType("application/pdf");

            if (!string.IsNullOrWhiteSpace(recipient))
            {
                intent.PutExtra(Intent.ExtraEmail, new[] { recipient.Trim() });
            }

            intent.PutExtra(Intent.ExtraSubject, subject ?? string.Empty);
            intent.PutExtra(Intent.ExtraText, body ?? string.Empty);

            if (!string.IsNullOrEmpty(attachmentPhysicalPath) && File.Exists(attachmentPhysicalPath))
            {
                var file = new Java.IO.File(attachmentPhysicalPath);
                var authority = $"{context.PackageName}.fileprovider";
                var uri = AndroidX.Core.Content.FileProvider.GetUriForFile(context, authority, file);
                intent.PutExtra(Intent.ExtraStream, uri);
                intent.AddFlags(ActivityFlags.GrantReadUriPermission);
            }

            var chooser = Intent.CreateChooser(intent, "Send CV via Email");
            chooser.AddFlags(ActivityFlags.NewTask | ActivityFlags.GrantReadUriPermission);
            context.StartActivity(chooser);
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[MobileEmailComposer] Android fallback failed: {ex.Message}");
            return false;
        }
#else
        // Fallback for Windows desktop or macOS
        try
        {
            var mailtoUri = string.IsNullOrWhiteSpace(recipient)
                ? $"mailto:?subject={Uri.EscapeDataString(subject ?? "")}&body={Uri.EscapeDataString(body ?? "")}"
                : $"mailto:{Uri.EscapeDataString(recipient.Trim())}?subject={Uri.EscapeDataString(subject ?? "")}&body={Uri.EscapeDataString(body ?? "")}";

            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(mailtoUri) { UseShellExecute = true });
                return true;
            }
            catch
            {
                await Launcher.Default.OpenAsync(new Uri(mailtoUri));
                return true;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[MobileEmailComposer] Shell mailto failed: {ex.Message}");
            return false;
        }
#endif
    }
}
