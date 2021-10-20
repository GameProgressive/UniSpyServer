using UniSpyServer.ServerBrowser.Abstraction.BaseClass;
using UniSpyServer.ServerBrowser.Entity.Enumerate;
using UniSpyServer.ServerBrowser.Entity.Structure.Misc;
using UniSpyServer.ServerBrowser.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Encryption;
using UniSpyServer.UniSpyLib.Extensions;

namespace UniSpyServer.ServerBrowser.Entity.Structure.Response
{
    /// <summary>
    /// Get a server's information
    /// </summary>
    public sealed class ServerInfoResponse : ServerListUpdateOptionResponseBase
    {
        private new ServerInfoResult _result => (ServerInfoResult)base._result;
        public ServerInfoResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            _serverListData.Add((byte)ResponseType.PushServerMessage);
            BuildServersInfo();
            // add message length here
            _serverListData.InsertRange(0, ByteTools.GetBytes((short)(_serverListData.Count + 2), true));
        }

        protected override void BuildServersInfo()
        {
            BuildServerInfoHeader(
                GameServerFlags.HasFullRulesFlag,
                _result.GameServerInfo);

            foreach (var kv in _result.GameServerInfo.ServerData.KeyValue)
            {
                _serverListData.AddRange(UniSpyEncoding.GetBytes(kv.Key));
                _serverListData.Add(StringFlag.StringSpliter);
                _serverListData.AddRange(UniSpyEncoding.GetBytes(kv.Value));
                _serverListData.Add(StringFlag.StringSpliter);
            }
            foreach (var player in _result.GameServerInfo.PlayerData.KeyValueList)
            {
                foreach (var kv in player)
                {
                    _serverListData.AddRange(UniSpyEncoding.GetBytes(kv.Key));
                    _serverListData.Add(StringFlag.StringSpliter);
                    _serverListData.AddRange(UniSpyEncoding.GetBytes(kv.Value));
                    _serverListData.Add(StringFlag.StringSpliter);
                }
            }
            foreach (var team in _result.GameServerInfo.TeamData.KeyValueList)
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