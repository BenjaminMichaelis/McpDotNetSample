// Program.cs
using McpDotNetSample.Asp.Services;
using McpDotNetSample.Asp.Tools;

var builder = WebApplication.CreateBuilder(args);

// Register the GitHub user repository
builder.Services.AddSingleton<IGithubUserRepository, InMemoryGithubUserRepository>();

builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();
var app = builder.Build();

// Initialize the static tool with the repository from DI
GithubUsersIKnowTool.Repository = app.Services.GetRequiredService<IGithubUserRepository>();

app.MapMcp();

app.Run();
