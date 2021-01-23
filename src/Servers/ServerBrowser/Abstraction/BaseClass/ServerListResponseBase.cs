using QueryReport.Entity.Structure;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Misc;
using ServerBrowser.Entity.Structure.Request;
using ServerBrowser.Network;
using System.Collections.Generic;
using System.Text;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Encryption;
using UniSpyLib.Extensions;

namespace ServerBrowser.Abstraction.BaseClass
{
    internal abstract class ServerListResponseBase : SBResponseBase
    {
        public byte[] PlainTextSendingBuffer { get; protected set; }
        private new ServerListRequestBase _request => (ServerListRequestBase)base._request;
        private new ServerListResultBase _result => (ServerListResultBase)base._result;
        public ServerListResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        protected override void BuildNormalResponse()
        {
            List<byte> serverListContext = new List<byte>();
            //Add crypt header
            serverListContext.AddRange(BuildCryptHeader());
            // Message length should be added here, between 2 line codes
            serverListContext.AddRange(_result.ClientRemoteIP);
            serverListContext.AddRange(ServerListRequestBase.HtonQueryReportDefaultPort);
            serverListContext.AddRange(SendingBuffer);
            PlainTextSendingBuffer = serverListContext.ToArray();
            GOAEncryption enc = new GOAEncryption(
                _result.GameSecretKey,
                _request.Challenge,
                SBServer.ServerChallenge);
            SendingBuffer = enc.Encrypt(PlainTextSendingBuffer);
        }
        protected static List<byte> BuildCryptHeader()
        {
            var data = new List<byte>();
            data.Add(2 ^ 0xEC);
            data.AddRange(new byte[] { 0, 0 });
            data.Add((byte)(SBServer.ServerChallenge.Length ^ 0xEA));
            data.AddRange(SBServer.BytesServerChallenge);
            return data;
        }
        protected abstract List<byte> BuildServersInfo();
        protected static List<byte> BuildServerInfoHeader(GameServerFlags flag, GameServerInfo serverInfo)
        {
            List<byte> header = new List<byte>();
            //add key flag
            header.Add((byte)flag);
            //we add server public ip here
            header.AddRange(ByteTools.GetIPBytes(serverInfo.RemoteQueryReportIP));
            //we check host port is standard port or not
            header.AddRange(CheckNonStandardPort(serverInfo));
            // now we check if there are private ip
            header.AddRange(CheckPrivateIP(serverInfo));
            // we check private port here
            header.AddRange(CheckPrivatePort(serverInfo));
            //we check icmp support here
            header.AddRange(CheckICMPSupport(serverInfo));
            return header;
        }
        protected static List<byte> CheckPrivateIP(GameServerInfo server)
        {
            var data = new List<byte>();
            string localIP = "";
            // now we check if there are private ip
            if (server.ServerData.KeyValue.ContainsKey("localip0"))
            {
                localIP = server.ServerData.KeyValue["localip0"];
            }
            else if (server.ServerData.KeyValue.ContainsKey("localip1"))
            {
                localIP = server.ServerData.KeyValue["localip1"];
            }

            if (localIP != "")
            {
                data[0] ^= (byte)GameServerFlags.PrivateIPFlag;
                byte[] bytesAddress = HtonsExtensions.IPStringToBytes(localIP);
                data.AddRange(bytesAddress);
            }
            return data;
        }
        protected static List<byte> CheckNonStandardPort(GameServerInfo server)
        {
            ///only dedicated server have different query report port and host port
            ///the query report port and host port are the same on peer server
            ///so we do not need to check this for peer server
            //we check host port is standard port or not
            var data = new List<byte>();
            if (server.ServerData.KeyValue.ContainsKey("hostport"))
            {
                if (server.ServerData.KeyValue["hostport"] != ""
                    && server.ServerData.KeyValue["hostport"] != "6500")
                {
                    data[0] ^= (byte)GameServerFlags.NonStandardPort;
                    byte[] htonPort =
                        HtonsExtensions.UshortPortToHtonBytes(
                            server.ServerData.KeyValue["hostport"]);
                    data.AddRange(htonPort);
                }
            }
            return data;
        }
        protected static List<byte> CheckPrivatePort(GameServerInfo server)
        {
            var data = new List<byte>();
            // we check private port here
            if (server.ServerData.KeyValue.ContainsKey("privateport"))
            {
                if (server.ServerData.KeyValue["privateport"] != "")
                {
                    data[0] ^= (byte)GameServerFlags.NonStandardPrivatePortFlag;
                    byte[] port = HtonsExtensions.PortToIntBytes(server.ServerData.KeyValue["privateport"]);
                    data.AddRange(port);
                }
            }
            return data;
        }
        protected static List<byte> CheckICMPSupport(GameServerInfo server)
        {
            List<byte> data = new List<byte>();
            data[0] ^= (byte)GameServerFlags.ICMPIPFlag;
            byte[] address = HtonsExtensions.IPStringToBytes(server.RemoteQueryReportIP);
            data.AddRange(address);
            return data;
        }
        protected List<byte> BuildUniqueValue()
        {
            var data = new List<byte>();
            data.Add(0);
            //because we are using NTS string so we do not have any value here
            return data;
        }
        protected List<byte> BuildServerKeys()
        {
            var data = new List<byte>();
            //we add the total number of the requested keys
            data.Add((byte)_request.Keys.Length);
            //then we add the keys
            foreach (var key in _request.Keys)
            {
                data.Add((byte)SBKeyType.String);
                data.AddRange(Encoding.ASCII.GetBytes(key));
                data.Add(SBStringFlag.StringSpliter);
            }
            return data;
        }

    }
}
