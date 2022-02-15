using System;
using System.Linq;
using UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass;
using UniSpyServer.Servers.ServerBrowser.Entity.Enumerate;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Misc;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Encryption;

namespace UniSpyServer.Servers.ServerBrowser.Entity.Structure.Response
{
    /// <summary>
    /// Get a server's information
    /// </summary>
    public sealed class ServerInfoResponse : ServerListUpdateOptionResponseBase
    {
        private new ServerInfoResult _result => (ServerInfoResult)base._result;
        public ServerInfoResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            _serverListData.Add((byte)ResponseType.PushServerMessage);
            BuildServerFullInfo();
            // add message length here
            var msgLength = BitConverter.GetBytes((ushort)(_serverListData.Count + 2)).Reverse().ToArray();
            _serverListData.InsertRange(0, msgLength);
            SendingBuffer = _serverListData.ToArray();
        }

        protected override void BuildServerFullInfo()
        {
            BuildServerInfoHeader(
                GameServerFlags.HasFullRulesFlag,
                _result.GameServerInfo);

            foreach (var kv in _result.GameServerInfo.ServerData)
            {
                _serverListData.AddRange(UniSpyEncoding.GetBytes(kv.Key));
                _serverListData.Add(StringFlag.StringSpliter);
                _serverListData.AddRange(UniSpyEncoding.GetBytes(kv.Value));
                _serverListData.Add(StringFlag.StringSpliter);
            }
            foreach (var player in _result.GameServerInfo.PlayerData)
            {
                foreach (var kv in player)
                {
                    _serverListData.AddRange(UniSpyEncoding.GetBytes(kv.Key));
                    _serverListData.Add(StringFlag.StringSpliter);
                    _serverListData.AddRange(UniSpyEncoding.GetBytes(kv.Value));
                    _serverListData.Add(StringFlag.StringSpliter);
                }
            }
            foreach (var team in _result.GameServerInfo.TeamData)
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