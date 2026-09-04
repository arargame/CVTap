using System.Text.RegularExpressions;
using CVTap.Shared.Services.Interfaces;

namespace CVTap.Shared.Services.Implementations;

public class TemplateEngine : ITemplateEngine
{
    private static readonly Regex VariableRegex = new(@"\{([A-Za-z0-9_]+)\}", RegexOptions.Compiled);

    private static readonly string[] SupportedVariables =
    [
        "Name",
        "Email",
        "Phone",
        "LinkedIn",
        "GitHub",
        "Website",
        "Company",
        "Position",
        "Date"
    ];

    public IReadOnlyList<string> GetSupportedVariables() => SupportedVariables;

    public string Render(string template, IDictionary<string, string?> variables)
    {
        if (string.IsNullOrEmpty(template))
        {
            return string.Empty;
        }

        // Case-insensitive dictionary lookup
        var varDict = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        foreach (var kvp in variables)
        {
            varDict[kvp.Key] = kvp.Value;
        }

        // Auto populate {Date} if not provided
        if (!varDict.ContainsKey("Date"))
        {
            varDict["Date"] = DateTime.Now.ToString("d MMMM yyyy");
        }

        return VariableRegex.Replace(template, match =>
        {
            var varName = match.Groups[1].Value;
            if (varDict.TryGetValue(varName, out var val) && val is not null)
            {
                return val;
            }
            // Unknown variable: keep original token or replace cleanly
            return match.Value;
        });
    }

    public IReadOnlyList<string> ExtractVariables(string template)
    {
        if (string.IsNullOrEmpty(template))
        {
            return Array.Empty<string>();
        }

        var matches = VariableRegex.Matches(template);
        var list = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (Match match in matches)
        {
            list.Add(match.Groups[1].Value);
        }

        return list.ToList();
    }
}
