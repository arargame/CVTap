using CVTap.Shared.Services.Interfaces;

namespace CVTap.Shared.Services.Implementations;

/// <summary>
/// Handles application theme state, switching, and persistent storage via UserProfileService.
/// </summary>
public class ThemeService : IThemeService
{
    private readonly IUserProfileService _profileService;
    private AppThemeMode _currentTheme = AppThemeMode.Dark;
    private bool _initialized;

    public ThemeService(IUserProfileService profileService)
    {
        _profileService = profileService;
    }

    public AppThemeMode CurrentTheme => _currentTheme;

    public string ThemeClass => _currentTheme == AppThemeMode.Light ? "theme-light" : "theme-dark";

    public bool IsDark => _currentTheme == AppThemeMode.Dark;

    public event Action? OnThemeChanged;

    public async Task InitializeAsync()
    {
        if (_initialized) return;

        try
        {
            var profile = await _profileService.GetProfileAsync();
            if (!string.IsNullOrWhiteSpace(profile.ThemeMode) &&
                profile.ThemeMode.Equals("light", StringComparison.OrdinalIgnoreCase))
            {
                _currentTheme = AppThemeMode.Light;
            }
            else
            {
                _currentTheme = AppThemeMode.Dark;
            }
            _initialized = true;
            OnThemeChanged?.Invoke();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ThemeService] Init theme failed: {ex.Message}");
        }
    }

    public async Task ToggleThemeAsync()
    {
        var newTheme = _currentTheme == AppThemeMode.Dark ? AppThemeMode.Light : AppThemeMode.Dark;
        await SetThemeAsync(newTheme);
    }

    public async Task SetThemeAsync(AppThemeMode mode)
    {
        _currentTheme = mode;
        _initialized = true;
        OnThemeChanged?.Invoke();

        try
        {
            var profile = await _profileService.GetProfileAsync();
            profile.ThemeMode = mode.ToString().ToLowerInvariant();
            await _profileService.SaveProfileAsync(profile);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ThemeService] Save theme failed: {ex.Message}");
        }
    }
}
