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

            CreateGameSpyAuthService();
        }
        public static void CreateGameSpyAuthService()
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
