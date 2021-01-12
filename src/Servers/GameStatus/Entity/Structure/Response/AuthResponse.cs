using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Structure.Request;
using GameStatus.Entity.Structure.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniSpyLib.Abstraction.BaseClass;

namespace GameStatus.Entity.Structure.Response
{
    internal sealed class AuthResponse : GSResponseBase
    {
        private new AuthResult _result => (AuthResult)base._result;
        private new AuthRequest _request => (AuthRequest)base._request;
        public AuthResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {
            SendingBuffer = @$"\sesskey\{_result.SessionKey}\lid\{ _request.OperationID}";
        }
    }
}
