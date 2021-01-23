using ServerBrowser.Abstraction.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniSpyLib.Abstraction.BaseClass;

namespace ServerBrowser.Entity.Structure.Response.ServerList
{
    /// <summary>
    /// Only get the servers IP
    /// </summary>
    internal sealed class NoServerListResponse : ServerListResponseBase
    {
        public NoServerListResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override List<byte> BuildServersInfo()
        {
            throw new NotImplementedException();
        }
    }
}
