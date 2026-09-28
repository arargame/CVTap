using CVTap.Shared.Data;
using CVTap.Shared.Models;
using CVTap.Shared.Services.Implementations;
using CVTap.Shared.Services.Interfaces;
using Xunit;

namespace CVTap.Tests;

public class UserProfileAndClipboardTests : IDisposable
{
    private readonly string _tempDbPath;
    private readonly CvTapDatabase _database;

    public UserProfileAndClipboardTests()
    {
        _tempDbPath = Path.Combine(Path.GetTempPath(), $"cvtap_profile_test_{Guid.NewGuid():N}.db3");
        _database = new CvTapDatabase(_tempDbPath);
    }

    [Fact]
    public async Task UserProfileService_SaveAndGetProfileAsync_ShouldPersistValues()
    {
        // Arrange
        var service = new UserProfileService(_database);
        var profile = new UserProfile
        {
            FullName = "Koray Arar",
            Email = "ararko_ray@hotmail.com",
            Phone = "+905384812797",
            LinkedIn = "https://www.linkedin.com/in/koray-arar-a5b70933/",
            GitHub = "https://github.com/arargame",
            Website = "www.mayhemcorporation.com"
        };

        // Act
        await service.SaveProfileAsync(profile);
        var retrieved = await service.GetProfileAsync();

        // Assert
        Assert.NotNull(retrieved);
        Assert.Equal("Koray Arar", retrieved.FullName);
        Assert.Equal("ararko_ray@hotmail.com", retrieved.Email);
        Assert.Equal("+905384812797", retrieved.Phone);
        Assert.Equal("https://www.linkedin.com/in/koray-arar-a5b70933/", retrieved.LinkedIn);
        Assert.Equal("https://github.com/arargame", retrieved.GitHub);
        Assert.Equal("www.mayhemcorporation.com", retrieved.Website);
    }

    [Fact]
    public async Task MockClipboardService_SetTextAsync_ShouldStoreClipboardText()
    {
        // Arrange
        var clipboard = new TestClipboardService();

        // Act
        var result = await clipboard.SetTextAsync("https://github.com/arargame");

        // Assert
        Assert.True(result);
        Assert.Equal("https://github.com/arargame", clipboard.CurrentText);
    }

    [Fact]
    public async Task ThemeService_ToggleThemeAsync_ShouldSwitchAndPersistTheme()
    {
        // Arrange
        var profileService = new UserProfileService(_database);
        var themeService = new ThemeService(profileService);

        // Act 1: Initialize (default dark)
        await themeService.InitializeAsync();
        Assert.Equal(AppThemeMode.Dark, themeService.CurrentTheme);
        Assert.Equal("theme-dark", themeService.ThemeClass);
        Assert.True(themeService.IsDark);

        // Act 2: Toggle to Light
        await themeService.ToggleThemeAsync();
        Assert.Equal(AppThemeMode.Light, themeService.CurrentTheme);
        Assert.Equal("theme-light", themeService.ThemeClass);
        Assert.False(themeService.IsDark);

        // Assert persistence in profile
        var profile = await profileService.GetProfileAsync();
        Assert.Equal("light", profile.ThemeMode);

        // Act 3: Toggle back to Dark
        await themeService.ToggleThemeAsync();
        Assert.Equal(AppThemeMode.Dark, themeService.CurrentTheme);
        Assert.Equal("theme-dark", themeService.ThemeClass);
        Assert.True(themeService.IsDark);
    }

    public void Dispose()
    {
        try
        {
            if (File.Exists(_tempDbPath))
            {
                File.Delete(_tempDbPath);
            }
        }
        catch
        {
        }
    }

    private class TestClipboardService : IClipboardService
    {
        public string? CurrentText { get; private set; }

        public Task<bool> SetTextAsync(string? text)
        {
            CurrentText = text;
            return Task.FromResult(true);
        }
    }
}
