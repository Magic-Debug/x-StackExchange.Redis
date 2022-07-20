using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
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

                }).UseStartup<Startup>();
    }
}
