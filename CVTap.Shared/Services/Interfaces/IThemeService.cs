namespace CVTap.Shared.Services.Interfaces;

/// <summary>
/// Supported application themes.
/// </summary>
public enum AppThemeMode
{
    Dark,
    Light
}

/// <summary>
/// Service abstraction for application theme management and persistence.
/// </summary>
public interface IThemeService
{
    /// <summary>
    /// Current theme mode (Dark or Light).
    /// </summary>
    AppThemeMode CurrentTheme { get; }

    /// <summary>
    /// CSS class name corresponding to the current theme ("theme-dark" or "theme-light").
    /// </summary>
    string ThemeClass { get; }

    /// <summary>
    /// Whether the current theme is Dark.
    /// </summary>
    bool IsDark { get; }

    /// <summary>
    /// Event fired when the theme changes.
    /// </summary>
    event Action? OnThemeChanged;

    /// <summary>
    /// Initializes the theme from persistent storage.
    /// </summary>
    Task InitializeAsync();

    /// <summary>
    /// Toggles between Dark and Light themes.
    /// </summary>
    Task ToggleThemeAsync();

    /// <summary>
    /// Explicitly sets the theme.
    /// </summary>
    Task SetThemeAsync(AppThemeMode mode);
}
