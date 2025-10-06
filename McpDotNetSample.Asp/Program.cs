// Program.cs
using McpDotNetSample.Asp.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Register the GitHub user repository
        builder.Services.AddSingleton<IGithubUserRepository, InMemoryGithubUserRepository>();

        builder.Services.AddMcpServer()
            .WithHttpTransport()
            .WithToolsFromAssembly();
        var app = builder.Build();

        app.MapMcp();

        app.Run();
    }
}
