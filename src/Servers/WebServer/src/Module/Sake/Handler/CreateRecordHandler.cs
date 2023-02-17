using System.IO;
using Newtonsoft.Json;
using UniSpy.Server.WebServer.Module.Sake.Abstraction;
using UniSpy.Server.WebServer.Module.Sake.Contract.Request;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.WebServer.Module.Sake.Handler
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