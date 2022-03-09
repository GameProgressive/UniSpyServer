using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UniSpyServer.Servers.WebServer.Entity.Structure;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.WebServer.Module.Sake.Abstraction
{
    public abstract class CmdHandlerBase : WebServer.Abstraction.CmdHandlerBase
    {
        protected new RequestBase _request => (RequestBase)base._request;
        protected string _sakeFilePath => $"./sake_storage/{_request.GameId}/{_request.TableId}/sake_storage.json";
        protected List<RecordFieldObject> _sakeData { get; private set; }
        protected CmdHandlerBase(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void RequestCheck()
        {
            base.RequestCheck();
            //todo get secretkey from database where gameid
        }
        protected override void DataOperation()
        {
            if (!File.Exists(_sakeFilePath))
            {
                new FileInfo(_sakeFilePath).Directory.Create();
            }
            else
            {
                _sakeData = JsonConvert.DeserializeObject<List<RecordFieldObject>>(File.ReadAllText(_sakeFilePath));
            }
        }
    }
}