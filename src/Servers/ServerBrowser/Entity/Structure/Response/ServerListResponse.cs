using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Misc;
using ServerBrowser.Entity.Structure.Request;
using ServerBrowser.Entity.Structure.Result;
using System.Collections.Generic;
using System.Text;
using UniSpyLib.Abstraction.BaseClass;

namespace ServerBrowser.Entity.Structure.Packet.Response
{
    internal sealed class ServerListResponse : UpdateOptionResponseBase
    {
        private new ServerListRequest _request => (ServerListRequest)base._request;
        private new ServerListResult _result => (ServerListResult)base._result;
        public ServerListResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
      
        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            List<byte> serverListContext = new List<byte>();
            foreach (var serverInfo in _result.GamerServerInfos)
            {
                serverListContext.AddRange(BuildServerKeys());
                //we use NTS string so total unique value list is 0
                serverListContext.AddRange(BuildUniqueValue());
                //add server infomation such as public ip etc.
                serverListContext.AddRange(
                    BuildServerInfoHeader(GameServerFlags.HasFullRulesFlag, serverInfo));
                //after all server information is added we add the end flag
                serverListContext.AddRange(SBStringFlag.AllServerEndFlag);
            }
        }
    }
}
