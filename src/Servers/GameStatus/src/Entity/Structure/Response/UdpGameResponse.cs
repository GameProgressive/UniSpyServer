using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Structure.Request;
using GameStatus.Entity.Structure.Result;
using System;
using UniSpyLib.Abstraction.BaseClass;

namespace GameStatus.Entity.Structure.Response
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
