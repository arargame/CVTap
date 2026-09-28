# CVTap - Android Storage Permissions & PDF File Picker Fix

## 1. Problem Description
On Android devices, users were unable to import or upload their CVs because:
- The file picker did not display PDF documents stored in the device file manager.
- The `AndroidManifest.xml` lacked essential external storage and document reading permissions.
- Modern Android OS versions (Android 11, 12, 13, 14, 15) and OEM file explorers (Samsung, Xiaomi, etc.) often tag downloaded PDFs with varying MIME types (`application/x-pdf`, `application/vnd.pdf`, or binary/octet streams), causing strict `application/pdf` filters to hide all files.
- The UI relied partly on an HTML `<InputFile>` element which behaves inconsistently across Android WebViews when handling `.pdf` file filters.

## 2. Implemented Solutions

### 2.1 Android Manifest Permissions & Queries
Updated `Platforms/Android/AndroidManifest.xml`:
- Added `READ_EXTERNAL_STORAGE` and `WRITE_EXTERNAL_STORAGE` with `maxSdkVersion="32"` for backward compatibility (Android 6.0 to 12).
- Added `READ_MEDIA_DOCUMENTS` for Android 13+ support.
- Added `<queries>` for `ACTION_OPEN_DOCUMENT` and `ACTION_GET_CONTENT` to comply with Android 11+ package visibility requirements.

### 2.2 Runtime Permission Handling
In `MobileCvStorageService.cs`:
- Implemented `EnsureStoragePermissionAsync` using `Permissions.CheckStatusAsync<Permissions.StorageRead>()` and `Permissions.RequestAsync<Permissions.StorageRead>()`.
- Checks runtime storage permission prior to launching the system file picker dialog.

### 2.3 Resilient PDF MIME Types & Fallback File Picker
- Expanded Android `FilePickerFileType` definition to include:
  - `application/pdf`
  - `application/x-pdf`
  - `application/vnd.pdf`
  - `text/pdf`
- Implemented an automatic fallback mechanism: if the OEM file manager fails or rejects the strict MIME filter, a general picker (`FilePicker.Default.PickAsync()`) is launched with explicit extension validation (`.pdf`), ensuring PDF files are always visible and selectable.

### 2.4 User Interface Enhancement (`MyCvsPage.razor`)
- Replaced ambiguous input controls in the upload card with an intuitive "Browse & Import PDF" button that directly triggers the native platform file picker.
- Corrected the web fallback `<InputFile>` MIME types to `application/pdf,application/x-pdf,.pdf`.

## 3. Verification
- Cross-platform solution build succeeded for all targets (`net9.0-android`, `net9.0-windows`, `net9.0-ios`, `net9.0-maccatalyst`).
- All 8 unit tests executed and passed.
