using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Result;
using System;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure.Response
{
    public sealed class UdpGameResponse : ResponseBase
    {
        private new UpdateGameRequest _request => (UpdateGameRequest)base._request;
        private new UdpGameResult _result => (UdpGameResult)base._result;
        public UdpGameResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            throw new NotImplementedException();
        }
    }
}
