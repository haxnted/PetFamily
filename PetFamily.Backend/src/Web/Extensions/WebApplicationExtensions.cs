using Serilog;
using Serilog.Events;

namespace Web.Extensions;

public static class WebApplicationExtensions
{
    public static void AddConfigureLogging(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Seq(
                builder.Configuration.GetConnectionString("Seq") ??
                throw new ArgumentNullException("Argument cannot be null")
            )
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
            .CreateLogger();
    }
}