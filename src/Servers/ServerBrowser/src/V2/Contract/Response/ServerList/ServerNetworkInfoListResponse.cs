using UniSpy.Server.ServerBrowser.V2.Abstraction.BaseClass;
using UniSpy.Server.ServerBrowser.V2.Enumerate;
using UniSpy.Server.ServerBrowser.V2.Aggregate.Misc;
using UniSpy.Server.ServerBrowser.V2.Contract.Result.ServerList;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.ServerBrowser.V2.Aggregate;

namespace UniSpy.Server.ServerBrowser.V2.Contract.Response.ServerList
{
    /// <summary>
    /// Get all server's full information
    /// </summary>
    public sealed class ServerNetworkInfoListResponse : ServerListUpdateOptionResponseBase
    {
        private new ServerNetworkInfoListResult _result => (ServerNetworkInfoListResult)base._result;
        public ServerNetworkInfoListResponse(ServerListUpdateOptionRequestBase request, ServerListUpdateOptionResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            base.Build();
            SendingBuffer = _serversInfoBuffer.ToArray();
        }
        protected override void BuildServersFullInfo()
        {
            foreach (var server in _result.ServersInfo)
            {
                var header = ServerInfoBuilder.BuildServerInfoHeader(
                                 GameServerFlags.HasFullRulesFlag,
                                 server);
                _serversInfoBuffer.AddRange(header);

                foreach (var kv in server.ServerData)
                {
                    _serversInfoBuffer.AddRange(UniSpyEncoding.GetBytes(kv.Key));
                    _serversInfoBuffer.Add(StringFlag.StringSpliter);
                    _serversInfoBuffer.AddRange(UniSpyEncoding.GetBytes(kv.Value));
                    _serversInfoBuffer.Add(StringFlag.StringSpliter);
                }
                foreach (var player in server.PlayerData)
                {
                    foreach (var kv in player)
                    {
                        _serversInfoBuffer.AddRange(UniSpyEncoding.GetBytes(kv.Key));
                        _serversInfoBuffer.Add(StringFlag.StringSpliter);
                        _serversInfoBuffer.AddRange(UniSpyEncoding.GetBytes(kv.Value));
                        _serversInfoBuffer.Add(StringFlag.StringSpliter);
                    }
                }
                foreach (var team in server.TeamData)
                {
                    foreach (var kv in team)
                    {
                        _serversInfoBuffer.AddRange(UniSpyEncoding.GetBytes(kv.Key));
                        _serversInfoBuffer.Add(StringFlag.StringSpliter);
                        _serversInfoBuffer.AddRange(UniSpyEncoding.GetBytes(kv.Value));
                        _serversInfoBuffer.Add(StringFlag.StringSpliter);
                    }
                }
            }
        }
    }
}
