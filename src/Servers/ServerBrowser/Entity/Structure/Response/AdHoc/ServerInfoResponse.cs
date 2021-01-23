using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Misc;
using ServerBrowser.Entity.Structure.Result;
using System.Collections.Generic;
using System.Text;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;

namespace ServerBrowser.Entity.Structure.Response
{
    /// <summary>
    /// Get a server's information
    /// </summary>
    internal sealed class ServerInfoResponse : ServerListResponseBase
    {
        private new ServerInfoResult _result => (ServerInfoResult)base._result;
        public ServerInfoResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        protected override void BuildNormalResponse()
        {
            List<byte> serverListContext = new List<byte>();
            serverListContext.Add((byte)SBServerResponseType.PushServerMessage);
            serverListContext.AddRange(BuildServersInfo());
            PlainTextSendingBuffer = serverListContext.ToArray();
            byte[] msgLength =
                ByteTools.GetBytes((short)(serverListContext.Count + 2), true);
            serverListContext.InsertRange(0, msgLength);
            SendingBuffer = serverListContext.ToArray();
        }
        protected override List<byte> BuildServersInfo()
        {
            List<byte> data = new List<byte>();
            data.AddRange(BuildServerInfoHeader(
                    GameServerFlags.HasFullRulesFlag,
                    _result.GameServerInfo));

            foreach (var kv in _result.GameServerInfo.ServerData.KeyValue)
            {
                data.AddRange(Encoding.ASCII.GetBytes(kv.Key));
                data.Add(SBStringFlag.StringSpliter);
                data.AddRange(Encoding.ASCII.GetBytes(kv.Value));
                data.Add(SBStringFlag.StringSpliter);
            }
            foreach (var player in _result.GameServerInfo.PlayerData.KeyValueList)
            {
                foreach (var kv in player)
                {
                    data.AddRange(Encoding.ASCII.GetBytes(kv.Key));
                    data.Add(SBStringFlag.StringSpliter);
                    data.AddRange(Encoding.ASCII.GetBytes(kv.Value));
                    data.Add(SBStringFlag.StringSpliter);
                }
            }
            foreach (var team in _result.GameServerInfo.TeamData.KeyValueList)
            {
                foreach (var kv in team)
                {
                    data.AddRange(Encoding.ASCII.GetBytes(kv.Key));
                    data.Add(SBStringFlag.StringSpliter);
                    data.AddRange(Encoding.ASCII.GetBytes(kv.Value));
                    data.Add(SBStringFlag.StringSpliter);
                }
            }
            return data;
        }
    }
}