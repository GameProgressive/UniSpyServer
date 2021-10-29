using System;
using System.IO;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.WebServer.Abstraction
{
    public abstract class SakeHandlerBase : CmdHandlerBase
    {
        protected new SakeRequestBase _request => (SakeRequestBase)base._request;
        protected string _sakeFilePath => $"./{_request.GameId}/{_request.TableId}/sake_storage.json";

        protected SakeHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
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
                File.Create(_sakeFilePath).Dispose();
            }
        }
    }
}