using CVTap.Shared.Data;
using CVTap.Shared.Models;
using CVTap.Shared.Services.Implementations;
using Xunit;

namespace CVTap.Tests;

public class DatabaseAndTemplateServiceTests : IDisposable
{
    private readonly string _tempDbPath;
    private readonly CvTapDatabase _database;

    public DatabaseAndTemplateServiceTests()
    {
        _tempDbPath = Path.Combine(Path.GetTempPath(), $"cvtap_test_{Guid.NewGuid():N}.db3");
        _database = new CvTapDatabase(_tempDbPath);
    }

    [Fact]
    public async Task EnsureInitializedAsync_ShouldSeedDefaultTemplatesAndProfile()
    {
        // Act
        await _database.EnsureInitializedAsync();

        // Assert
        var templates = await _database.Connection.Table<EmailTemplate>().ToListAsync();
        Assert.NotEmpty(templates);
        Assert.Contains(templates, t => t.Language == "EN" && t.IsDefault);
        Assert.Contains(templates, t => t.Language == "TR");

        var profile = await _database.Connection.Table<UserProfile>().FirstOrDefaultAsync();
        Assert.NotNull(profile);
    }

    [Fact]
    public async Task EmailTemplateService_DuplicateAsync_ShouldCreateExactCopy()
    {
        // Arrange
        await _database.EnsureInitializedAsync();
        var service = new EmailTemplateService(_database);
        var templates = await service.GetAllAsync();
        var original = templates.First();

        // Act
        var copy = await service.DuplicateAsync(original.Id);

        // Assert
        Assert.NotEqual(original.Id, copy.Id);
        Assert.Equal($"{original.Title} (Copy)", copy.Title);
        Assert.Equal(original.SubjectTemplate, copy.SubjectTemplate);
        Assert.Equal(original.BodyTemplate, copy.BodyTemplate);
        Assert.False(copy.IsDefault);
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
}
