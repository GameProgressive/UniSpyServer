using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Result;
using System;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure.Response
{
    public sealed class UpdateGameResponse : ResponseBase
    {
        private new UpdateGameRequest _request => (UpdateGameRequest)base._request;
        private new UpdateGameResult _result => (UpdateGameResult)base._result;
        public UpdateGameResponse(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            throw new NotImplementedException();
        }
    }
}
