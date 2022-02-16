using System.IO;
using Newtonsoft.Json;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.Servers.WebServer.Entity.Structure.Request.Sake;
using UniSpyServer.Servers.WebServer.Entity.Contract;

namespace UniSpyServer.Servers.WebServer.Handler.CmdHandler.Sake
{
    [HandlerContract("CreateRecord")]
    public class CreateRecordHandler : Abstraction.Sake.CmdHandlerBase
    {
        protected new CreateRecordRequest _request => (CreateRecordRequest)base._request;
        public CreateRecordHandler(IClient client, IRequest request) : base(client, request)
        {
        }
        protected override void DataOperation()
        {
            base.DataOperation();
            var jsonStr = JsonConvert.SerializeObject(_request.Values);
            File.WriteAllText(_sakeFilePath, jsonStr);
        }
    }
}