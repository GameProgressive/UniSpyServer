using System;
using System.Linq;
using UniSpy.Server.ServerBrowser.Abstraction.BaseClass;
using UniSpy.Server.ServerBrowser.Enumerate;
using UniSpy.Server.ServerBrowser.Aggregate.Misc;
using UniSpy.Server.ServerBrowser.Contract.Result;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.ServerBrowser.Aggregate;

namespace UniSpy.Server.ServerBrowser.Contract.Response
{
    /// <summary>
    /// Get a server's information
    /// </summary>
    public sealed class UpdateServerInfoResponse : AdHocResponseBase
    {
        public UpdateServerInfoResponse(AdHocResult result) : base(result)
        {
        }

        public override void Build()
        {
            _buffer.Add((byte)ResponseType.PushServerMessage);
            BuildSingleServerFullInfo();
            // add message length here
            var msgLength = BitConverter.GetBytes((ushort)(_buffer.Count + 2)).Reverse().ToArray();
            // we add the message length at the start
            _buffer.InsertRange(0, msgLength);
            SendingBuffer = _buffer.ToArray();
        }

        private void BuildSingleServerFullInfo()
        {
            var header = ServerInfoBuilder.BuildServerInfoHeader(
                GameServerFlags.HasFullRulesFlag,
                _result.GameServerInfo);
            _buffer.AddRange(header);
            if (_result.GameServerInfo.ServerData is not null)
            {
                foreach (var kv in _result.GameServerInfo.ServerData)
                {
                    _buffer.AddRange(UniSpyEncoding.GetBytes(kv.Key));
                    _buffer.Add(StringFlag.StringSpliter);
                    _buffer.AddRange(UniSpyEncoding.GetBytes(kv.Value));
                    _buffer.Add(StringFlag.StringSpliter);
                }
            }
            if (_result.GameServerInfo.PlayerData is not null)
            {
                foreach (var player in _result.GameServerInfo.PlayerData)
                {
                    foreach (var kv in player)
                    {
                        _buffer.AddRange(UniSpyEncoding.GetBytes(kv.Key));
                        _buffer.Add(StringFlag.StringSpliter);
                        _buffer.AddRange(UniSpyEncoding.GetBytes(kv.Value));
                        _buffer.Add(StringFlag.StringSpliter);
                    }
                }
            }
            if (_result.GameServerInfo.TeamData is not null)
            {
                foreach (var team in _result.GameServerInfo.TeamData)
                {
                    foreach (var kv in team)
                    {
                        _buffer.AddRange(UniSpyEncoding.GetBytes(kv.Key));
                        _buffer.Add(StringFlag.StringSpliter);
                        _buffer.AddRange(UniSpyEncoding.GetBytes(kv.Value));
                        _buffer.Add(StringFlag.StringSpliter);
                    }
                }
            }
        }
    }
}