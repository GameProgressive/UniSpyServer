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
    internal sealed class ServerNetworkInfoListResponse : ServerListUpdateOptionResponseBase
    {
        public ServerNetworkInfoListResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            SendingBuffer = _serverListData.ToArray();
        }
        protected override void BuildServersInfo()
        {
            throw new NotImplementedException();
        }
    }
}
