using UniSpy.Server.QueryReport.Application;
using UniSpy.Server.QueryReport.V1.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.V1.Contract.Request;

namespace UniSpy.Server.QueryReport.V1.Handler.CmdHandler
{
    /// <summary>
    /// After sending heartbeat response, the game server will send validate to qr server to indicate his identity
    /// </summary>
    public sealed class ValidateHandler : CmdHandlerBase
    {
        public ValidateHandler(Client client, ValidateRequest request) : base(client, request)
        {
        }
        protected override void DataOperation()
        {
            throw new System.Exception();
        }
    }
}