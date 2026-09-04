namespace CVTap.Shared.Services.Interfaces;

public interface ITemplateEngine
{
    string Render(string template, IDictionary<string, string?> variables);
    IReadOnlyList<string> ExtractVariables(string template);
    IReadOnlyList<string> GetSupportedVariables();
}
