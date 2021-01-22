using ServerBrowser.Abstraction.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniSpyLib.Abstraction.BaseClass;

namespace ServerBrowser.Entity.Structure.Response
{
    internal sealed class PushUpdateResponse : UpdateOptionResponseBase
    {
        public PushUpdateResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {

            base.BuildNormalResponse();
        }
    }
}
