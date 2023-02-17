using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Contract.Request;
using UniSpy.Server.GameStatus.Contract.Result;
using System;

namespace UniSpy.Server.GameStatus.Contract.Response
{
    public sealed class SetPlayerDataResponse : ResponseBase
    {
        private new SetPlayerDataResult _result => (SetPlayerDataResult)base._result;
        private new SetPlayerDataRequest _request => (SetPlayerDataRequest)base._request;
        public SetPlayerDataResponse(UniSpy.Server.Core.Abstraction.BaseClass.RequestBase request, UniSpy.Server.Core.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            throw new NotImplementedException();
        }
    }
}
