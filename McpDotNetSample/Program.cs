using System.ComponentModel;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using ModelContextProtocol.Server;

namespace McpDotNetSample;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Logging.AddConsole(consoleLogOptions =>
            consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace);
        builder.Services
            .AddMcpServer()
            .WithStdioServerTransport()
            .WithToolsFromAssembly();
        await builder.Build().RunAsync();
    }
}

[McpServerToolType]
public static class AddTool
{
    [McpServerTool(Name = "add"), Description("Adds two numbers.")]
    public static string Add(int left, int right)
    {
        return $"The sum of {left} and {right} is {left + right}";
    }
}