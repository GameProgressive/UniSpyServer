using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Result;
using System;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure.Response
{
    public sealed class CreateNewGameResponse : ResponseBase
    {
        private new CreateNewGameResult _result => (CreateNewGameResult)base._result;
        private new CreateNewGameRequest _request => (CreateNewGameRequest)base._request;
        public CreateNewGameResponse(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            throw new NotImplementedException();
        }
    }
}
