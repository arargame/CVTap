# CVTap
> **"Send your CV in a few taps."**

[![.NET 9.0](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![.NET MAUI Blazor Hybrid](https://img.shields.io/badge/MAUI-Blazor%20Hybrid-purple?logo=dotnet)](https://learn.microsoft.com/aspnet/core/blazor/hybrid/)
[![Privacy First](https://img.shields.io/badge/Privacy-100%25%20Local--Only-success)](https://github.com/arargame/CVTap)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

**CVTap** is a mobile-first, privacy-focused utility application built with **.NET 9 MAUI Blazor Hybrid** and **Blazor Web**. It solves a common pain point for job seekers: quickly selecting a tailored CV, attaching it to an email template with personalized tokens, and opening the device's native email client (Gmail, Outlook, Apple Mail) or system share sheet (WhatsApp, Telegram, Drive) in under 10 seconds.

---

## 🔒 Privacy-First & Zero-Tracking Philosophy

> *"Your CV never leaves your phone until you choose to send it."*

* **No Accounts / No Cloud**: Zero sign-up, zero logins, zero servers.
* **No SMTP Credentials**: We never ask for your email password or OAuth tokens. The app hands off the drafted email and PDF attachment directly to your device's native email app.
* **Zero Sensitive Store Permissions**: Uses standard document picker (`FilePicker`) and isolated local application storage (`FileSystem.AppDataDirectory`). No broad storage permissions required.
* **Local SQLite Database**: All metadata, templates, and history are stored locally on the device using SQLite (`sqlite-net-pcl`).

---

## ✨ Features

* 🚀 **Fast 3-Tap Send Flow**:
  1. Pick one of your saved CVs (defaults automatically).
  2. Enter recipient, company name, and position.
  3. Select template, verify the live preview, and tap **[ EMAIL CV ]** or **[ SHARE CV ]**.
* 📄 **Multi-CV Management**: Keep different resumes for different roles (e.g. *.NET Developer CV*, *Game Developer CV*, *English CV*, *Turkish CV*).
* ✉️ **Dynamic Email Template Engine**:
  * Deterministic variable substitution for `{Name}`, `{Email}`, `{Phone}`, `{LinkedIn}`, `{GitHub}`, `{Website}`, `{Company}`, `{Position}`, and `{Date}`.
  * Interactive one-tap variable chips in the template editor.
  * Out-of-the-box pre-seeded templates in English and Turkish.
  * Duplicate, customize, and set default templates.
* 🕒 **Application History**: Keep track of every sent/opened application with timestamp, company, position, and status toggles (*Composer Opened*, *Applied*).
* 📱 **Native OS Integration**:
  * Android `FileProvider` (`content://`) & `<queries>` for robust Gmail/Outlook attachment sharing.
  * Android & iOS system Share Sheet for WhatsApp, Telegram, etc.
  * Windows Desktop Shell fallbacks.
* 🌐 **Cross-Platform Shared Codebase**: UI and business logic live in a single **Razor Class Library** (`CVTap.Shared`), shared across Mobile (.NET MAUI) and Web (Blazor Web).

---

## 🏗️ Architecture & Project Structure

```
CVTap/
├── CVTap.sln
│
├── CVTap.Shared/                       # Razor Class Library (Shared UI & Core)
│   ├── Models/                         # CvProfile, EmailTemplate, UserProfile, ApplicationRecord
│   ├── Services/                       # ITemplateEngine, ICvStorageService, IEmailComposerService, etc.
│   ├── Data/                           # CvTapDatabase (SQLite with lazy migration & seeders)
│   ├── Pages/                          # Home.razor, MyCvsPage.razor, TemplatesPage.razor, EditTemplatePage.razor, ProfilePage.razor, HistoryPage.razor
│   ├── Layout/                         # MainLayout.razor, NavBottom.razor, NotificationToast.razor
│   └── wwwroot/                        # Modern mobile-first dark theme CSS (app.css)
│
├── CVTap/                              # .NET MAUI Blazor Hybrid (Android / iOS / Windows)
│   ├── Platforms/Android/              # FileProvider (file_paths.xml) & queries declarations
│   ├── Services/                       # Native Android/MAUI implementations
│   └── MauiProgram.cs                  # MAUI dependency injection container
│
├── CVTap.Web/                          # Blazor Web App (InteractiveServer / Web)
│   ├── Services/                       # Web Share API & browser mailto services
│   └── Program.cs                      # ASP.NET Core web host configuration
│
└── CVTap.Tests/                        # xUnit automated unit tests
    ├── TemplateEngineTests.cs          # Deterministic token replacement tests
    └── DatabaseAndTemplateServiceTests.cs # SQLite initialization & CRUD tests
```

---

## 🚀 Getting Started

### Prerequisites
* [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (Version 9.0.300+ or latest)
* For Android: .NET MAUI Android Workload (`dotnet workload install maui-android`)
* Visual Studio 2022 (v17.12+) or VS Code with C# Dev Kit

### 1. Clone the Repository
```bash
git clone https://github.com/arargame/CVTap.git
cd CVTap
```

### 2. Run Automated Tests
```bash
dotnet test CVTap.Tests/CVTap.Tests.csproj
```

### 3. Run the Web Project
```bash
dotnet run --project CVTap.Web/CVTap.Web.csproj
```

### 4. Run the Mobile App (Android)
```bash
dotnet build CVTap/CVTap.csproj -f net9.0-android
```
Or select **CVTap** in Visual Studio with an Android Emulator / physical device connected and press **F5**.

---

## 🧪 Testing Summary

* **xUnit Suite**: 6 tests covering template token extraction, case-insensitivity, safety against unknown tokens, SQLite seeding, and template duplication.
* **Build Targets**: Verified with 0 errors across `net9.0`, `net9.0-android`, and `net9.0-windows10.0.19041.0`.

---

## 📄 License

This project is licensed under the MIT License.
