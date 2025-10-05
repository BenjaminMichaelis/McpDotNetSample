using System.ComponentModel;
using System.Text.Json;

using ModelContextProtocol.Server;

namespace McpDotNetSample;

[McpServerToolType]
public static class SnippetTool
{
    // Root directory for snippet storage (temp folder to keep things ephemeral per-machine)
    private static readonly string SnippetRoot = Path.Combine(Path.GetTempPath(), "mcp_snippets");

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    // Simple DTO / record for serialization
    public sealed record Snippet(string Name, string Content, DateTimeOffset SavedAt);

    private static string SanitizeFileName(string name)
    {
        // Replace invalid file name chars with '_'
        return string.Join('_', name.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries));
    }

    private static string GetFilePath(string name) => Path.Combine(SnippetRoot, SanitizeFileName(name) + ".json");

    private static void EnsureRoot()
    {
        Directory.CreateDirectory(SnippetRoot);
    }

    [McpServerTool, Description("Retrieves a code snippet by name. Returns the snippet content.")]
    public static string GetSnippet(string name)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name);

        EnsureRoot();
        string path = GetFilePath(name);
        if (!File.Exists(path))
            throw new FileNotFoundException($"Snippet '{name}' was not found.");

        string json = File.ReadAllText(path);
        Snippet snippet = JsonSerializer.Deserialize<Snippet>(json, JsonOptions) ?? throw new InvalidOperationException("Failed to deserialize snippet.");
        return snippet.Content;
    }

    [McpServerTool, Description("Saves (creates or overwrites) a code snippet by name and returns metadata.")]
    public static Snippet SaveSnippet(string name, string content)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(content);

        EnsureRoot();
        var path = GetFilePath(name);
        var snippet = new Snippet(name, content, DateTimeOffset.UtcNow);

        var json = JsonSerializer.Serialize(snippet, JsonOptions);
        File.WriteAllText(path, json);

        return snippet;
    }

    [McpServerTool, Description("Lists available snippet names in the collection.")]
    public static string[] ListSnippets()
    {
        EnsureRoot();
        var files = Directory.EnumerateFiles(SnippetRoot, "*.json", SearchOption.TopDirectoryOnly);
        return [.. files.Select(f => Path.GetFileNameWithoutExtension(f))];
    }

    [McpServerTool, Description("Deletes a code snippet by name.")]
    public static void DeleteSnippet(string name)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name);

        EnsureRoot();
        var path = GetFilePath(name);

        if (File.Exists(path))
        {
            File.Delete(path);
        }
        else
        {
            throw new FileNotFoundException($"Snippet '{name}' was not found.");
        }
    }
}