using CVTap.Shared.Services.Interfaces;

namespace CVTap.Shared.Services.Implementations;

public class ToastService : IToastService
{
    public event Action<string, ToastLevel>? OnShow;

    public void Show(string message, ToastLevel level = ToastLevel.Info)
    {
        OnShow?.Invoke(message, level);
    }

    public void Success(string message) => Show(message, ToastLevel.Success);
    public void Error(string message) => Show(message, ToastLevel.Danger);
    public void Info(string message) => Show(message, ToastLevel.Info);
}
