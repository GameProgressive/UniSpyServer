using UniSpyServer.GameStatus.Abstraction.BaseClass;
using UniSpyServer.GameStatus.Entity.Structure.Request;
using UniSpyServer.GameStatus.Entity.Structure.Result;
using System;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.GameStatus.Entity.Structure.Response
{
    public sealed class SetPDResponse : ResponseBase
    {
        private new SetPDResult _result => (SetPDResult)base._result;
        private new SetPlayerDataRequest _request => (SetPlayerDataRequest)base._request;
        public SetPDResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            throw new NotImplementedException();
        }
    }
}
