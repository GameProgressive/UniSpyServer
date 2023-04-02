using UniSpy.Server.ServerBrowser.V1.Abstraction.BaseClass;
using UniSpy.Server.ServerBrowser.V1.Application;

namespace UniSpy.Server.ServerBrowser.V1.Handler.CmdHandler
{
    public class ListHandler : CmdHandlerBase
    {
        public ListHandler(Client client, RequestBase request) : base(client, request)
        {
        }
        protected override void RequestCheck()
        {
            throw new System.NotImplementedException();
            // base.RequestCheck();
        }
    }
}