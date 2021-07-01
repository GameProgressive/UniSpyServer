using ServerBrowser.Abstraction.BaseClass;
using System;
using UniSpyLib.Abstraction.BaseClass;

namespace ServerBrowser.Entity.Structure.Response.ServerList
{
    /// <summary>
    /// Only get the servers IP
    /// </summary>
    internal sealed class ServerNetworkInfoListResponse : ServerListUpdateOptionResponseBase
    {
        public ServerNetworkInfoListResponse(UniSpyRequest request, UniSpyResult result) : base(request, result)
        {
        }
        public override void Build()
        {
            base.Build();
            SendingBuffer = _serverListData.ToArray();
        }
        protected override void BuildServersInfo()
        {
            throw new NotImplementedException();
        }
    }
}
