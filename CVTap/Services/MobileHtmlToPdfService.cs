using CVTap.Shared.Services.Interfaces;

namespace CVTap.Services;

public class MobileHtmlToPdfService : IHtmlToPdfService
{
    public Task<string> GenerateAsync(string html, string outputFileName, CancellationToken cancellationToken = default)
    {
        // Phase 2 will wire Android off-screen WebView.CreatePrintDocumentAdapter
        // and iOS WKWebView.CreatePdfAsync
        throw new NotImplementedException("Offline HTML to PDF generation is planned for Phase 2.");
    }
}
