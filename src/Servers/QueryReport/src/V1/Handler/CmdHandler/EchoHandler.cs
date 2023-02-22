using UniSpy.Server.Core.Abstraction.Interface;
using System;
namespace UniSpy.Server.QueryReport.V1.Handler.CmdHandler
{
    public class EchoHandler : V1.Abstraction.BaseClass.CmdHandlerBase
    {
        public EchoHandler(IClient client, IRequest request) : base(client, request)
        {
        }
        protected override void DataOperation()
        {
            base.DataOperation();
            throw new NotImplementedException();
        }
    }
}