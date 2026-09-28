using Microsoft.Extensions.Logging;
using CVTap.Shared.Data;
using CVTap.Shared.Services.Implementations;
using CVTap.Shared.Services.Interfaces;
using CVTap.Services;

namespace CVTap;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Initialize SQLite Database in AppDataDirectory
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "cvtap.db3");
        var database = new CvTapDatabase(dbPath);
        builder.Services.AddSingleton(database);

        // Core & Domain Services
        builder.Services.AddSingleton<IToastService, ToastService>();
        builder.Services.AddSingleton<ITemplateEngine, TemplateEngine>();
        builder.Services.AddSingleton<IEmailTemplateService, EmailTemplateService>();
        builder.Services.AddSingleton<IUserProfileService, UserProfileService>();
        builder.Services.AddSingleton<IApplicationHistoryService, ApplicationHistoryService>();

        // Native / Platform Services
        builder.Services.AddSingleton<ICvStorageService, MobileCvStorageService>();
        builder.Services.AddSingleton<IEmailComposerService, MobileEmailComposerService>();
        builder.Services.AddSingleton<IShareService, MobileShareService>();
        builder.Services.AddSingleton<IHtmlToPdfService, MobileHtmlToPdfService>();
        builder.Services.AddSingleton<IClipboardService, MobileClipboardService>();

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
