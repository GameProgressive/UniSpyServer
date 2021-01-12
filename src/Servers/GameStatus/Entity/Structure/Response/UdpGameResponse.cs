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
    internal sealed class UdpGameResponse : GSResponseBase
    {
        private new UdpGameRequest _request => (UdpGameRequest)base._request;
        private new UdpGameResult _result => (UdpGameResult)base._result;
        public UdpGameResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {
            throw new NotImplementedException();
        }
    }
}
