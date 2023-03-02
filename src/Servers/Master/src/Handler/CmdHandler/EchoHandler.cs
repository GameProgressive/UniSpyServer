using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Master.Contract.Result;
using UniSpy.Server.Master.Contract.Response;
using UniSpy.Server.Master.Abstraction.BaseClass;

namespace UniSpy.Server.Master.Handler.CmdHandler
{
    /// <summary>
    /// Keep alive request
    /// </summary>
    public sealed class EchoHandler : CmdHandlerBase
    {
        private new EchoResult _result { get => (EchoResult)base._result; set => base._result = value; }
        public EchoHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new EchoResult();
        }
        protected override void DataOperation()
        {
            _result.Challenge = HeartbeatHandler.Challenge;
        }
        protected override void ResponseConstruct()
        {
            _response = new EchoResponse(_request, _result);
        }
    }
}