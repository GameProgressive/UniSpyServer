using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis;
using UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Misc;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Request;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Encryption;

namespace UniSpyServer.Servers.ServerBrowser.Entity.Structure.Packet.Response
{
    public sealed class ServerMainListResponse : ServerListUpdateOptionResponseBase
    {
        private new ServerListRequest _request => (ServerListRequest)base._request;
        private new ServerMainListResult _result => (ServerMainListResult)base._result;
        public ServerMainListResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            // we add the other header
            base.Build();
            // we add the server keys
            BuildServerKeys();
            //we use NTS string so total unique value list is 0
            BuildUniqueValue();
            //add server infomation such as public ip etc.
            BuildServersInfo();

            SendingBuffer = _serverListData.ToArray();
        }

        protected override void BuildServersInfo()
        {
            foreach (var serverInfo in _result.GameServerInfos)
            {
                //we check the server
                //This is the way we can not crash by some
                //fake server
                if (IsSkipThisServer(serverInfo))
                {
                    continue;
                }
                BuildServerInfoHeader(_result.Flag, serverInfo);
                foreach (var key in _request.Keys)
                {
                    _serverListData.Add(StringFlag.NTSStringFlag);
                    _serverListData.AddRange(UniSpyEncoding.GetBytes(serverInfo.ServerData.KeyValue[key]));
                    _serverListData.Add(StringFlag.StringSpliter);
                }
            }
            //after all server information is added we add the end flag
            _serverListData.AddRange(StringFlag.AllServerEndFlag);
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
