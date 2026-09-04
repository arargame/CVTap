using CVTap.Shared.Data;
using CVTap.Shared.Services.Implementations;

namespace CVTap.Web.Services;

public class WebCvStorageService : BaseCvStorageService
{
    public WebCvStorageService(CvTapDatabase db)
        : base(db, GetWebStoragePath())
    {
    }

    private static string GetWebStoragePath()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var dir = Path.Combine(localAppData, "CVTapWeb");
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        return dir;
    }
}
