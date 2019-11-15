using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RetroSpyWeb;
using System.IO;

namespace Handler.AuthHandler.Program
{
    public class AuthServiceCreator
    {
        public static void CreateHTTPAuthService()
        {
            var host = new WebHostBuilder()
                    .UseKestrel(x => x.AllowSynchronousIO = true)
                    .UseUrls("http://*.auth.pubsvs.gamespy.com:80")
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
        public static void CreateHTTPSAuthService()
        {
            var host = new WebHostBuilder()
                        .UseKestrel(x => x.AllowSynchronousIO = true)
                        .UseUrls("https://*.auth.pubsvs.gamespy.com:443")
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