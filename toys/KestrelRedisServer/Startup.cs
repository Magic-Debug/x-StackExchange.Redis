using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis.Server;

namespace KestrelRedisServer
{
    public class Startup : IDisposable
    {
        private static RespServer RespServer => new MemoryCacheRedisServer(10);

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Add(new ServiceDescriptor(typeof(RespServer), RespServer));
        }

        public void Dispose()
        {
            RespServer.Dispose();
            GC.SuppressFinalize(this);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            RespServer.Shutdown.ContinueWith((t, s) =>
            {
                try
                {   // if the resp server is shutdown by a client: stop the kestrel server too
                    if (t.Result == RespServer.ShutdownReason.ClientInitiated)
                    {
                        ((IHostApplicationLifetime)s).StopApplication();
                    }
                }
                catch { /* Don't go boom on shutdown */ }
            }, lifetime);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.Run(handler: context =>
            {
                string stat = RespServer.GetStats();
                return context.Response.WriteAsync(stat);
            });
        }
    }
}
