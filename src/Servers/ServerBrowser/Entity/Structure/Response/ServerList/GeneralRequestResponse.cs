using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Redis;
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
    internal sealed class GeneralRequestResponse : ServerListResponseBase
    {
        private new ServerListRequest _request => (ServerListRequest)base._request;
        private new GeneralRequestResult _result => (GeneralRequestResult)base._result;
        public GeneralRequestResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {
            List<byte> serverListContext = new List<byte>();
            serverListContext.AddRange(BuildServerKeys());
            //we use NTS string so total unique value list is 0
            serverListContext.AddRange(BuildUniqueValue());
            //add server infomation such as public ip etc.
            serverListContext.AddRange(BuildServersInfo());
            //after all server information is added we add the end flag
            serverListContext.AddRange(SBStringFlag.AllServerEndFlag);
            SendingBuffer = serverListContext.ToArray();
            // Finally we add the other header
            base.BuildNormalResponse();
        }
        protected override List<byte> BuildServersInfo()
        {
            List<byte> data = new List<byte>();
            foreach (var serverInfo in _result.GameServerInfos)
            {
                //we check the server
                //This is the way we can not crash by some
                //fake server
                if (IsSkipThisServer(serverInfo))
                {
                    continue;
                }
                data.AddRange(BuildServerInfoHeader((GameServerFlags)_result.Flag, serverInfo));
                foreach (var key in _request.Keys)
                {
                    data.Add(SBStringFlag.NTSStringFlag);
                    data.AddRange(Encoding.ASCII.GetBytes(serverInfo.ServerData.KeyValue[key]));
                    data.Add(SBStringFlag.StringSpliter);
                }
            }
            return data;
        }

        private bool IsSkipThisServer(GameServerInfo serverInfo)
        {
            foreach (var key in _request.Keys)
            {
                //do not skip empty value
                if (!serverInfo.ServerData.KeyValue.ContainsKey(key))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
