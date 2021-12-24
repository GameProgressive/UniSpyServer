using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Result;
using System;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure.Response
{
    public sealed class SetPlayerDataResponse : ResponseBase
    {
        private new SetPlayerDataResult _result => (SetPlayerDataResult)base._result;
        private new SetPlayerDataRequest _request => (SetPlayerDataRequest)base._request;
        public SetPlayerDataResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            throw new NotImplementedException();
        }
    }
}
