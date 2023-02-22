using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.QueryReport.V1.Handler.CmdHandler
{
    public class HeartBeatHandler : V1.Abstraction.BaseClass.CmdHandlerBase
    {
        public HeartBeatHandler(IClient client, IRequest request) : base(client, request)
        {
        }
    }
}