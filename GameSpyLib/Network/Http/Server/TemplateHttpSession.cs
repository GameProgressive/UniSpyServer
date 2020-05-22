using System.Text;
using GameSpyLib.Logging;
using NetCoreServer;
using Serilog.Events;

namespace GameSpyLib.Network.Http.Server
{
    public abstract class TemplateHttpSession : HttpSession
    {
        public TemplateHttpSession(HttpServer server) : base(server)
        {
        }

        protected override void OnReceivedRequest(HttpRequest request)
        {
            switch (request.Method)
            {
                case "POST":
                    OnReceivedPostRequest(request);
                    break;
                case "GET":
                    break;
                case "HEAD":
                    break;
                case "DELETE":
                    break;
                case "OPTIONS":
                    break;
                case "TRACE":
                    break;
                default:
                    LogWriter.UnknownDataRecieved(request.Body);
                    break;
            }

        }

        protected abstract void OnReceivedPostRequest(HttpRequest request);

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            LogWriter.ToLog(LogEventLevel.Debug,
               $"[Recv] {Encoding.ASCII.GetString(buffer, 0, (int)size)}");
            base.OnReceived(buffer, offset, size);
        }
        public override bool SendAsync(string text)
        {
            LogWriter.ToLog(LogEventLevel.Debug,
                $"[Send] {text}");
            return base.SendAsync(text);
        }
    }
}
