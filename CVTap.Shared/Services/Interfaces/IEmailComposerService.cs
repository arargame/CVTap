namespace CVTap.Shared.Services.Interfaces;

public interface IEmailComposerService
{
    Task<bool> CanComposeEmailAsync();
    Task<bool> ComposeEmailAsync(string recipient, string subject, string body, string? attachmentPhysicalPath = null);
}
