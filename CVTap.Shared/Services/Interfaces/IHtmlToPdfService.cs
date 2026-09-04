namespace CVTap.Shared.Services.Interfaces;

public interface IHtmlToPdfService
{
    Task<string> GenerateAsync(string html, string outputFileName, CancellationToken cancellationToken = default);
}
