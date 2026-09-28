# CVTap - Theme Management (Dark & Light Modes)

## 1. Overview
This technical document describes the design and implementation of application-wide theme switching (Dark & Light modes) with persistent state and dynamic header controls in CVTap.

## 2. Architecture & Design Patterns
- **Abstraction (`IThemeService`)**:
  - Implements the Dependency Inversion Principle, isolating UI components from the persistent store.
  - Exposes `CurrentTheme`, `ThemeClass`, `IsDark`, and `OnThemeChanged` event.
  - Supports `ToggleThemeAsync()` and `SetThemeAsync(AppThemeMode)`.
- **Persistence (`UserProfile.ThemeMode`)**:
  - Theme preference is stored in SQLite via `UserProfileService`, persisting across application restarts.
- **Dynamic CSS Variable Theming (`app.css`)**:
  - `:root, .theme-dark`: Default deep slate and indigo palette.
  - `.theme-light`: High-contrast, clean light slate and indigo palette.
  - Smooth transitions (`0.25s ease`) for colors, backgrounds, and cards.
- **Header Toggle Control (`MainLayout.razor`)**:
  - Located in the top-right corner of the app header next to the "Local Only" status badge.
  - Displays `☀️ Light` or `🌙 Dark` in desktop mode, and adapts to a compact icon (`☀️`/`🌙`) on mobile screens.

## 3. Key Files
- `CVTap.Shared/Services/Interfaces/IThemeService.cs`: Theme contract.
- `CVTap.Shared/Services/Implementations/ThemeService.cs`: Theme state, notifications, and persistence.
- `CVTap.Shared/Models/UserProfile.cs`: Added `ThemeMode` column for SQLite.
- `CVTap.Shared/Layout/MainLayout.razor`: Layout integration and header toggle button.
- `CVTap.Shared/wwwroot/app.css`: Variables, theme definitions, and `.btn-theme-toggle` styling.
- `CVTap/MauiProgram.cs` & `CVTap.Web/Program.cs`: Dependency injection registration.
- `CVTap.Tests/UserProfileAndClipboardTests.cs`: Unit test coverage for toggling and profile persistence.

## 4. Verification
- Built for all target platforms (`net9.0-android`, `net9.0-windows`, `net9.0-ios`, `net9.0-maccatalyst`).
- 9/9 unit tests passing.
