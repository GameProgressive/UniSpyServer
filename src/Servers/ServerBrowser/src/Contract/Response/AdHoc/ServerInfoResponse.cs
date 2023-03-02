using System;
using System.Linq;
using UniSpy.Server.ServerBrowser.Abstraction.BaseClass;
using UniSpy.Server.ServerBrowser.Enumerate;
using UniSpy.Server.ServerBrowser.Aggregate.Misc;
using UniSpy.Server.ServerBrowser.Contract.Result;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.ServerBrowser.Contract.Request;

namespace UniSpy.Server.ServerBrowser.Contract.Response
{
    /// <summary>
    /// Get a server's information
    /// </summary>
    public sealed class ServerInfoResponse : ServerListUpdateOptionResponseBase
    {
        private new ServerInfoResult _result => (ServerInfoResult)base._result;
        public ServerInfoResponse(ServerInfoRequest request, ServerInfoResult result) : base(request, result)
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
            if (_result.GameServerInfo.ServerData is not null)
            {
                foreach (var kv in _result.GameServerInfo.ServerData)
                {
                    _serverListData.AddRange(UniSpyEncoding.GetBytes(kv.Key));
                    _serverListData.Add(StringFlag.StringSpliter);
                    _serverListData.AddRange(UniSpyEncoding.GetBytes(kv.Value));
                    _serverListData.Add(StringFlag.StringSpliter);
                }
            }
            if (_result.GameServerInfo.PlayerData is not null)
            {
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
            }
            if (_result.GameServerInfo.TeamData is not null)
            {
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
}