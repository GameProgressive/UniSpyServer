using UniSpyServer.Servers.ServerBrowser.V2.Abstraction.BaseClass;
using UniSpyServer.Servers.ServerBrowser.V2.Entity.Enumerate;
using UniSpyServer.Servers.ServerBrowser.V2.Entity.Structure.Misc;
using UniSpyServer.Servers.ServerBrowser.V2.Entity.Structure.Result.ServerList;
using UniSpyServer.UniSpyLib.Encryption;

namespace UniSpyServer.Servers.ServerBrowser.V2.Entity.Structure.Response.ServerList
{
    /// <summary>
    /// Get all server's full information
    /// </summary>
    public sealed class ServerNetworkInfoListResponse : ServerListUpdateOptionResponseBase
    {
        private new ServerNetworkInfoListResult _result => (ServerNetworkInfoListResult)base._result;
        public ServerNetworkInfoListResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            base.Build();
            SendingBuffer = _serverListData.ToArray();
        }
        protected override void BuildServerFullInfo()
        {
            foreach (var server in _result.ServersInfo)
            {
                BuildServerInfoHeader(
                                GameServerFlags.HasFullRulesFlag,
                                server);

                foreach (var kv in server.ServerData)
                {
                    _serverListData.AddRange(UniSpyEncoding.GetBytes(kv.Key));
                    _serverListData.Add(StringFlag.StringSpliter);
                    _serverListData.AddRange(UniSpyEncoding.GetBytes(kv.Value));
                    _serverListData.Add(StringFlag.StringSpliter);
                }
                foreach (var player in server.PlayerData)
                {
                    foreach (var kv in player)
                    {
                        _serverListData.AddRange(UniSpyEncoding.GetBytes(kv.Key));
                        _serverListData.Add(StringFlag.StringSpliter);
                        _serverListData.AddRange(UniSpyEncoding.GetBytes(kv.Value));
                        _serverListData.Add(StringFlag.StringSpliter);
                    }
                }
                foreach (var team in server.TeamData)
                {
                    foreach (var kv in team)
                    {
                        _serverListData.AddRange(UniSpyEncoding.GetBytes(kv.Key));
                        _serverListData.Add(StringFlag.StringSpliter);
                        _serverListData.AddRange(UniSpyEncoding.GetBytes(kv.Value));
                        _serverListData.Add(StringFlag.StringSpliter);
                    }
                }
            }

        }

    }
}