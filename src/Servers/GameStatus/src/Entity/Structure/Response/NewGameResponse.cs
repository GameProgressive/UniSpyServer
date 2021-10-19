using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Structure.Request;
using GameStatus.Entity.Structure.Result;
using System;
using UniSpyLib.Abstraction.BaseClass;

namespace GameStatus.Entity.Structure.Response
{
    public sealed class NewGameResponse : ResponseBase
    {
        private new NewGameResult _result => (NewGameResult)base._result;
        private new NewGameRequest _request => (NewGameRequest)base._request;
        public NewGameResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            throw new NotImplementedException();
        }
    }
}
