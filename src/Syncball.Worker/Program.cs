using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Syncball.Worker;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Syncball Worker starting.");
        var builder = Host.CreateApplicationBuilder(args);
        ConfigureServices(builder);

        var host = builder.Build();
        using (host)
        {
            host.StartAsync().GetAwaiter().GetResult();
            Console.WriteLine("Syncball Worker running.");
            host.WaitForShutdown();
        }
    }

    private static void ConfigureServices(HostApplicationBuilder builder)
    {
        builder.Services.AddHostedService<Application>();
        builder.Services.AddLogging(configure => configure.AddConsole());
    }
}