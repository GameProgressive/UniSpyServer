using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Result;
using System;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure.Response
{
    public sealed class NewGameResponse : ResponseBase
    {
        private new CreateNewGameDataResult _result => (CreateNewGameDataResult)base._result;
        private new NewGameRequest _request => (NewGameRequest)base._request;
        public NewGameResponse(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            throw new NotImplementedException();
        }
    }
}
