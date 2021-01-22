using System.Collections.Generic;
using System.Text;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Network;
using UniSpyLib.Abstraction.BaseClass;
using ServerBrowser.Entity.Structure.Result;
using ServerBrowser.Entity.Structure.Request;
using QueryReport.Entity.Structure;
using ServerBrowser.Entity.Enumerate;
using UniSpyLib.Extensions;

namespace ServerBrowser.Entity.Structure.Packet.Response
{
    internal sealed class ServerListResponse : UpdateOptionResponseBase
    {
        private new ServerListRequest _request => (ServerListRequest)base._request;
        private new ServerListResult _result => (ServerListResult)base._result;
        public ServerListResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
    }
}
