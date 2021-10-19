using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Structure.Request;
using GameStatus.Entity.Structure.Result;
using System;
using UniSpyLib.Abstraction.BaseClass;

namespace GameStatus.Entity.Structure.Response
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
