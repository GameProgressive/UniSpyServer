using System.Collections.Generic;
using System.Text;
using ServerBrowser.Network;
using UniSpyLib.Abstraction.BaseClass;
using ServerBrowser.Entity.Structure.Result;
using ServerBrowser.Entity.Structure.Request;
using QueryReport.Entity.Structure;
using ServerBrowser.Entity.Enumerate;
using UniSpyLib.Extensions;

namespace ServerBrowser.Abstraction.BaseClass
{
    internal abstract class UpdateOptionResponseBase : SBResponseBase
    {
        public byte[] PlainTextSendingBuffer { get; protected set; }
        private new ServerListRequest _request => (ServerListRequest)base._request;
        private new ServerListResult _result => (ServerListResult)base._result;
        public UpdateOptionResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        protected override void BuildNormalResponse()
        {
            List<byte> serverListContext = new List<byte>();
            #region Crypt header
            //Add crypt header
            // we add the message length here
            serverListContext.Add(2 ^ 0xEC);
            serverListContext.AddRange(new byte[] { 0, 0 });
            serverListContext.Add((byte)(SBServer.ServerChallenge.Length ^ 0xEA));
            serverListContext.AddRange(SBServer.BytesServerChallenge);
            #endregion
            serverListContext.AddRange(_result.ClientRemoteIP);
            serverListContext.AddRange(ServerListRequest.HtonQueryReportDefaultPort);
            serverListContext.AddRange(SendingBuffer);
            SendingBuffer = serverListContext.ToArray();
        }
        public static void CheckPrivateIP(List<byte> header, GameServerInfo server)
        {
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
            if (localIP == "")
            {
                return;
            }

            header[0] ^= (byte)GameServerFlags.PrivateIPFlag;
            byte[] address = HtonsExtensions.IPStringToBytes(localIP);
            header.AddRange(address);
        }

        public static void CheckNonStandardPort(List<byte> header, GameServerInfo server)
        {
            ///only dedicated server have different query report port and host port
            ///the query report port and host port are the same on peer server
            ///so we do not need to check this for peer server
            //we check host port is standard port or not
            if (!server.ServerData.KeyValue.ContainsKey("hostport"))
            {
                return;
            }
            if (server.ServerData.KeyValue["hostport"] == "")
            {
                return;
            }

            if (server.ServerData.KeyValue["hostport"] != "6500")
            {
                header[0] ^= (byte)GameServerFlags.NonStandardPort;
                //we do not need htons here
                byte[] port =
                     HtonsExtensions.PortToIntBytes(
                         server.ServerData.KeyValue["hostport"]);
                byte[] htonPort =
                    HtonsExtensions.UshortPortToHtonBytes(
                        server.ServerData.KeyValue["hostport"]);
                header.AddRange(htonPort);
            }
        }

        public static void CheckPrivatePort(List<byte> header, GameServerInfo server)
        {
            // we check private port here
            if (!server.ServerData.KeyValue.ContainsKey("privateport"))
            {
                return;
            }
            if (server.ServerData.KeyValue["privateport"] == "")
            {
                return;
            }
            header[0] ^= (byte)GameServerFlags.NonStandardPrivatePortFlag;

            byte[] port =
                HtonsExtensions.PortToIntBytes(
                    server.ServerData.KeyValue["privateport"]);

            header.AddRange(port);
        }

        public static void CheckICMPSupport(List<byte> header, GameServerInfo server)
        {
            header[0] ^= (byte)GameServerFlags.ICMPIPFlag;
            byte[] address = HtonsExtensions.IPStringToBytes(server.RemoteQueryReportIP);
            header.AddRange(address);
        }

    }
}
