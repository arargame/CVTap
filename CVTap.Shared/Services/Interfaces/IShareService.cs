namespace CVTap.Shared.Services.Interfaces;

public interface IShareService
{
    Task<bool> ShareFileAsync(string title, string physicalFilePath, string? textContent = null);
}
