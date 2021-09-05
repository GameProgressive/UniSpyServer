using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Misc;
using ServerBrowser.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Encryption;
using UniSpyLib.Extensions;

namespace ServerBrowser.Entity.Structure.Response
{
    /// <summary>
    /// Get a server's information
    /// </summary>
    internal sealed class ServerInfoResponse : ServerListUpdateOptionResponseBase
    {
        private new ServerInfoResult _result => (ServerInfoResult)base._result;
        public ServerInfoResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            _serverListData.Add((byte)SBServerResponseType.PushServerMessage);
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
                _serverListData.Add(SBStringFlag.StringSpliter);
                _serverListData.AddRange(UniSpyEncoding.GetBytes(kv.Value));
                _serverListData.Add(SBStringFlag.StringSpliter);
            }
            foreach (var player in _result.GameServerInfo.PlayerData.KeyValueList)
            {
                foreach (var kv in player)
                {
                    _serverListData.AddRange(UniSpyEncoding.GetBytes(kv.Key));
                    _serverListData.Add(SBStringFlag.StringSpliter);
                    _serverListData.AddRange(UniSpyEncoding.GetBytes(kv.Value));
                    _serverListData.Add(SBStringFlag.StringSpliter);
                }
            }
            foreach (var team in _result.GameServerInfo.TeamData.KeyValueList)
            {
                foreach (var kv in team)
                {
                    _serverListData.AddRange(UniSpyEncoding.GetBytes(kv.Key));
                    _serverListData.Add(SBStringFlag.StringSpliter);
                    _serverListData.AddRange(UniSpyEncoding.GetBytes(kv.Value));
                    _serverListData.Add(SBStringFlag.StringSpliter);
                }
            }
        }
    }
}