using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

namespace RetroSpyWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public void CreateGameSpyAuthService()
        {
            var host = new WebHostBuilder()
                    .UseKestrel(x => x.AllowSynchronousIO = true)
                    .UseUrls("http://*:4040")
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>()
                    .ConfigureLogging(x =>
                    {
                        x.AddDebug();
                        x.AddConsole();
                    })
                    .Build();

            host.Run();
        }
    }
}
