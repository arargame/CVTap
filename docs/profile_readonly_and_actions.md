# CVTap - Profile Readonly Mode, Copy & Edit Feature

## 1. Overview
This technical document describes the implementation of the Profile Readonly mode along with inline `Copy` and `Edit` action buttons on each profile input field.

## 2. Requirements & Architecture
- **Readonly State On Save**: When the user saves their profile (or returns to an existing saved profile), all fields default to a clean readonly presentation.
- **Copy Button (📋)**: Allows one-tap copying of individual field values to the clipboard with real-time feedback (toast and inline status indicator).
- **Edit Button (✏️)**: Exits readonly mode for that specific field, enabling immediate inline editing.
- **Cross-Platform Clipboard Abstraction**: Follows SOLID principles (Interface Segregation & Dependency Inversion) via `IClipboardService`.
  - `MobileClipboardService`: Native .NET MAUI Clipboard integration (`Microsoft.Maui.ApplicationModel.DataTransfer.Clipboard`).
  - `WebClipboardService`: JavaScript Interop integration with `navigator.clipboard`.
- **Responsive Layout**: Works seamlessly across desktop and mobile screens. On smaller screens, action buttons dynamically adapt to save space while maintaining touch targets.

## 3. Key Modified & Added Files
- `CVTap.Shared/Services/Interfaces/IClipboardService.cs`: Service abstraction for cross-platform clipboard.
- `CVTap.Shared/Services/Interfaces/IToastService.cs`: Added `Warning(string)` helper.
- `CVTap.Shared/Services/Implementations/ToastService.cs`: Implemented `Warning(string)`.
- `CVTap/Services/MobileClipboardService.cs`: MAUI native implementation.
- `CVTap.Web/Services/WebClipboardService.cs`: Blazor web implementation.
- `CVTap/MauiProgram.cs` & `CVTap.Web/Program.cs`: Registered `IClipboardService` in DI containers.
- `CVTap.Shared/Pages/ProfilePage.razor`: UI and interaction handling for readonly state, inline edit, copy, and lock.
- `CVTap.Shared/wwwroot/app.css`: Styling for `.input-action-group`, `.btn-input-action`, and mobile responsive adjustments.
- `CVTap.Tests/UserProfileAndClipboardTests.cs`: Unit tests for persistence and clipboard service contracts.

## 4. Verification
- Solution built without errors across target frameworks.
- Unit tests executed and verified passing (8/8 tests passed).
