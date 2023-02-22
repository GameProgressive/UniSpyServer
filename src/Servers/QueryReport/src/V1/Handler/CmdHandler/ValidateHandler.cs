using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.QueryReport.V1.Handler.CmdHandler
{
    public class ValidateHandler : V1.Abstraction.BaseClass.CmdHandlerBase
    {
        public ValidateHandler(IClient client, IRequest request) : base(client, request)
        {
        }
    }
}