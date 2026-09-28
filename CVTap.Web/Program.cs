using CVTap.Web.Components;
using CVTap.Shared.Services;
using CVTap.Shared.Data;
using CVTap.Shared.Services.Implementations;
using CVTap.Shared.Services.Interfaces;
using CVTap.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// FormFactor
builder.Services.AddSingleton<IFormFactor, FormFactor>();

// Initialize SQLite Database in LocalApplicationData
var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
var dbDir = Path.Combine(localAppData, "CVTapWeb");
if (!Directory.Exists(dbDir))
{
    Directory.CreateDirectory(dbDir);
}
var dbPath = Path.Combine(dbDir, "cvtap.db3");
var database = new CvTapDatabase(dbPath);
builder.Services.AddSingleton(database);

// Core & Domain Services
builder.Services.AddScoped<IToastService, ToastService>();
builder.Services.AddSingleton<ITemplateEngine, TemplateEngine>();
builder.Services.AddSingleton<IEmailTemplateService, EmailTemplateService>();
builder.Services.AddSingleton<IUserProfileService, UserProfileService>();
builder.Services.AddSingleton<IApplicationHistoryService, ApplicationHistoryService>();

// Web Platform Services
builder.Services.AddSingleton<ICvStorageService, WebCvStorageService>();
builder.Services.AddScoped<IEmailComposerService, WebEmailComposerService>();
builder.Services.AddScoped<IShareService, WebShareService>();
builder.Services.AddSingleton<IHtmlToPdfService, WebHtmlToPdfService>();
builder.Services.AddScoped<IClipboardService, WebClipboardService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(CVTap.Shared._Imports).Assembly);

app.Run();
