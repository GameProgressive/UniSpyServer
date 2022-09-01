using System.IO;
using Newtonsoft.Json;
using UniSpyServer.Servers.WebServer.Module.Sake.Abstraction;
using UniSpyServer.Servers.WebServer.Module.Sake.Structure.Request;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.WebServer.Module.Sake.Handler
{
    
    public class CreateRecordHandler : CmdHandlerBase
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