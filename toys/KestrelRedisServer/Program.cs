using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets;
using Microsoft.Extensions.Logging;
using System.Net;

namespace KestrelRedisServer
{
    public static class Program
    {
        public static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel((WebHostBuilderContext context, KestrelServerOptions options) =>
                {
                    // Moved to SocketTransportOptions.UnsafePreferInlineScheduling = true;
                    //options.ApplicationSchedulingMode = SchedulingMode.Inline;

                    // HTTP 5000
                    options.ListenLocalhost(5000);

                    //Redis TCP 6379
                    options.Listen(IPAddress.Any, 6379, (listenOptions) =>
                    {
                        listenOptions.UseConnectionLogging();
                        listenOptions.UseConnectionHandler<RedisConnectionHandler>();
                    });

                })
            .ConfigureLogging(factory =>
           {
               factory.AddConsole();
               factory.AddDebug();
               LogLevel logLevel = LogLevel.Debug;
               factory.SetMinimumLevel(logLevel);
               factory.AddFilter("Microsoft", LogLevel.Debug);
           })
           .UseSockets((SocketTransportOptions options) =>
            {
                options.IOQueueCount = 17;
                options.NoDelay = false;
            }).UseStartup<Startup>();
    }
}
