using CVTap.Shared.Services.Implementations;
using Xunit;

namespace CVTap.Tests;

public class TemplateEngineTests
{
    private readonly TemplateEngine _engine = new();

    [Fact]
    public void Render_ShouldReplaceKnownVariables_Deterministically()
    {
        // Arrange
        var template = "Hello, I am applying for {Position} at {Company}. Best, {Name}";
        var vars = new Dictionary<string, string?>
        {
            { "Position", ".NET Engineer" },
            { "Company", "Ubisoft" },
            { "Name", "Arar Games" }
        };

        // Act
        var result = _engine.Render(template, vars);

        // Assert
        Assert.Equal("Hello, I am applying for .NET Engineer at Ubisoft. Best, Arar Games", result);
    }

    [Fact]
    public void Render_ShouldBeCaseInsensitive_ForVariableNames()
    {
        // Arrange
        var template = "Position: {position}, Company: {COMPANY}, Name: {name}";
        var vars = new Dictionary<string, string?>
        {
            { "Position", "Architect" },
            { "Company", "Mojang" },
            { "Name", "Alex" }
        };

        // Act
        var result = _engine.Render(template, vars);

        // Assert
        Assert.Equal("Position: Architect, Company: Mojang, Name: Alex", result);
    }

    [Fact]
    public void Render_ShouldPreserveUnknownVariables_WithoutCrashing()
    {
        // Arrange
        var template = "Applying for {Position} at {Company} with {UnknownVariable}";
        var vars = new Dictionary<string, string?>
        {
            { "Position", "Dev" },
            { "Company", "Google" }
        };

        // Act
        var result = _engine.Render(template, vars);

        // Assert
        Assert.Equal("Applying for Dev at Google with {UnknownVariable}", result);
    }

    [Fact]
    public void ExtractVariables_ShouldReturnAllUniqueVariableTokens()
    {
        // Arrange
        var template = "Dear {Company}, I am {Name}. Position is {Position}. Contact {Name} at {Email}.";

        // Act
        var extracted = _engine.ExtractVariables(template);

        // Assert
        Assert.Equal(4, extracted.Count);
        Assert.Contains("Company", extracted);
        Assert.Contains("Name", extracted);
        Assert.Contains("Position", extracted);
        Assert.Contains("Email", extracted);
    }
}
