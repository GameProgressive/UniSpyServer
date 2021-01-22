using System.Collections.Generic;
using System.Text;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Misc;
using UniSpyLib.Abstraction.BaseClass;
using ServerBrowser.Entity.Structure.Result;
using UniSpyLib.Logging;
using UniSpyLib.Extensions;

namespace ServerBrowser.Entity.Structure.Packet.Response
{
    /// <summary>
    /// Get a server's information
    /// </summary>
    internal sealed class ServerInfoResponse : UpdateOptionResponseBase
    {
        private new ServerInfoResult _result => (ServerInfoResult)base._result;
        public ServerInfoResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        protected override void BuildNormalResponse()
        {
            List<byte> serverListContext = new List<byte>();
            serverListContext.Add((byte)SBServerResponseType.PushServerMessage);
            GenerateServerInfo(serverListContext);
            LogWriter.ToLog("[Plain] " +
               StringExtensions.ReplaceUnreadableCharToHex(serverListContext.ToArray()));
            byte[] msgLength =
                ByteTools.GetBytes((short)(serverListContext.Count + 2), true);
        }
        private void GenerateServerInfo(List<byte> data)
        {
            data.AddRange(
                UpdateOptionHandlerBase.GenerateServerInfoHeader(
                    GameServerFlags.HasFullRulesFlag, _result.GameServerInfo));

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
        }
    }

}