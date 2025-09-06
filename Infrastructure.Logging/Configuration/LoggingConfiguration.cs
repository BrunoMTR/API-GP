using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Grafana.Loki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Logging.Configuration
{
    public class LoggingConfiguration
    {
        public static void Configure(HostBuilderContext context, LoggerConfiguration configuration)
        {
            configuration
            .ReadFrom.KeyValuePairs(context.Configuration.AsEnumerable())
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(
                path: "C:\\Users\\Dell\\Documents\\PF\\API-GP\\logs\\log-.txt",
                rollingInterval: RollingInterval.Day,
                shared: true,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}"
    );
        }
    }
}
