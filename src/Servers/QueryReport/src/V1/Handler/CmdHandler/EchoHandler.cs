using UniSpy.Server.Core.Abstraction.Interface;
using System;
using UniSpy.Server.QueryReport.V1.Contract.Result;
using UniSpy.Server.QueryReport.V1.Contract.Response;

namespace UniSpy.Server.QueryReport.V1.Handler.CmdHandler
{
    /// <summary>
    /// Keep alive request
    /// </summary>
    public class EchoHandler : V1.Abstraction.BaseClass.CmdHandlerBase
    {
        private new EchoResult _result { get => (EchoResult)base._result; set => base._result = value; }
        public EchoHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new EchoResult();
        }
        protected override void DataOperation()
        {
            // base.DataOperation();
            _result.Challenge = HeartbeatHandler.Challenge;
            throw new NotImplementedException();
            // todo update expiretime on redis
        }
        protected override void ResponseConstruct()
        {
            _response = new EchoResponse(_request, _result);
        }
    }
}