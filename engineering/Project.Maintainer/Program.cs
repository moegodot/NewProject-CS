using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;
using Autofac;
using Serilog;

namespace Project.Maintainer;

sealed class Program
{
    public const string UniqueBrandMarker = "^await GodotAsync()$";
    public const string UniqueBrandStartMarker = "BEGIN";
    public const string UniqueBrandEndMarker = "END";

    static async Task<int> Main(string[] args)
    {
        var log = new LoggerConfiguration()
                  .WriteTo.Console(
                      outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
                      formatProvider: null)
                  .MinimumLevel.Verbose()
                  .CreateLogger();

        var builder = new ContainerBuilder();
        builder.RegisterAssemblyModules(Assembly.GetCallingAssembly());
        builder.RegisterInstance(log).AsSelf().As<ILogger>().SingleInstance();
        await using var container = builder.Build();

        var logger = container.Resolve<ILogger>().ForContext<Program>();

        try
        {
            var maintainTasks = container.Resolve<IEnumerable<IMaintainTask>>();
            var maintaining = container.Resolve<Maintaining>();

            foreach (var task in maintainTasks)
            {
                logger.Information("execute {IMaintainTask}", task.GetType().Name);
                await task.Maintain(maintaining);
            }
        }
        catch (Exception exception)
        {
            logger.Fatal(exception, "catch an exception");
            return 1;
        }

        return 0;
    }
}
