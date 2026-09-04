using CVTap.Shared.Services.Interfaces;

namespace CVTap.Web.Services;

public class WebHtmlToPdfService : IHtmlToPdfService
{
    public Task<string> GenerateAsync(string html, string outputFileName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Web HTML-to-PDF will be implemented in Phase 2.");
    }
}
