using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.QueryReport.V1.Abstraction.BaseClass;

namespace UniSpy.Server.QueryReport.V1.Handler.CmdHandler
{
    /// <summary>
    /// After sending heartbeat response, the game server will send validate to qr server to indicate his identity
    /// </summary>
    public class ValidateHandler : CmdHandlerBase
    {
        public ValidateHandler(IClient client, IRequest request) : base(client, request)
        {
        }
        protected override void DataOperation()
        {
            throw new System.Exception();
        }
    }
}