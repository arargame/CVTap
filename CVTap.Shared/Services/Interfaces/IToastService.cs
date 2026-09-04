namespace CVTap.Shared.Services.Interfaces;

public enum ToastLevel
{
    Info,
    Success,
    Warning,
    Danger
}

public interface IToastService
{
    event Action<string, ToastLevel>? OnShow;
    void Show(string message, ToastLevel level = ToastLevel.Info);
    void Success(string message);
    void Error(string message);
    void Info(string message);
}
