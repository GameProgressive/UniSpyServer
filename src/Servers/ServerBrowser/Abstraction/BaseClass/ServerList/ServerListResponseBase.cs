using System;
using System.Collections.Generic;
using System.Text;
using QueryReport.Entity.Structure.Redis;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Misc;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;

namespace ServerBrowser.Abstraction.BaseClass
{
    internal abstract class ServerListResponseBase : SBResponseBase
    {
        protected new ServerListRequestBase _request => (ServerListRequestBase)base._request;
        protected new ServerListResultBase _result => (ServerListResultBase)base._result;
        public byte[] PlainTextSendingBuffer { get; protected set; }
        protected List<byte> _serverListContext;
        public ServerListResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
            _serverListContext = new List<byte>();
        }

        protected override void BuildNormalResponse()
        {
            //Add crypt header
            BuildCryptHeader();
            _serverListContext.InsertRange(13, ByteTools.GetBytes((short)_serverListContext.Count, true));
            // Message length should be added here, between 2 line codes
            _serverListContext.InsertRange(15, _result.ClientRemoteIP);
            _serverListContext.InsertRange(19, ServerListRequestBase.HtonQueryReportDefaultPort);
        }

        protected void BuildCryptHeader()
        {
            // cryptHeader have 14 bytes, when we encrypt data we need skip the first 14 bytes
            var cryptHeader = new List<byte>();
            cryptHeader.Add(2 ^ 0xEC);
            // add message length
            cryptHeader.AddRange(new byte[] { 0, 0 });
            cryptHeader.Add((byte)(SBConstants.ServerChallenge.Length ^ 0xEA));
            cryptHeader.AddRange(Encoding.ASCII.GetBytes(SBConstants.ServerChallenge));
            _serverListContext.InsertRange(0, cryptHeader);
        }
        protected abstract void BuildServersInfo();
        protected void BuildServerInfoHeader(GameServerFlags? flag, GameServerInfo serverInfo)
        {
            List<byte> header = new List<byte>();
            //add key flag
            header.Add((byte)flag);
            //we add server public ip here
            header.AddRange(ByteTools.GetIPBytes(serverInfo.RemoteQueryReportIP));
            //we check host port is standard port or not
            CheckNonStandardPort(header, serverInfo);
            // now we check if there are private ip
            CheckPrivateIP(header, serverInfo);
            // we check private port here
            CheckPrivatePort(header, serverInfo);
            //we check icmp support here
            CheckICMPSupport(header, serverInfo);

            _serverListContext.AddRange(header);
        }
        protected void CheckPrivateIP(List<byte> header, GameServerInfo server)
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

            if (localIP != "")
            {
                header[0] ^= (byte)GameServerFlags.PrivateIPFlag;
                byte[] bytesAddress = HtonsExtensions.IPStringToBytes(localIP);
                header.AddRange(bytesAddress);
            }
        }

        protected void CheckNonStandardPort(List<byte> header, GameServerInfo server)
        {
            ///only dedicated server have different query report port and host port
            ///the query report port and host port are the same on peer server
            ///so we do not need to check this for peer server
            //we check host port is standard port or not
            if (server.ServerData.KeyValue.ContainsKey("hostport"))
            {
                if (server.ServerData.KeyValue["hostport"] != ""
                    && server.ServerData.KeyValue["hostport"] != "6500")
                {
                    header[0] ^= (byte)GameServerFlags.NonStandardPort;
                    byte[] htonPort =
                        HtonsExtensions.UshortPortToHtonBytes(
                            server.ServerData.KeyValue["hostport"]);
                    header.AddRange(htonPort);
                }
            }
        }
        protected void CheckPrivatePort(List<byte> header, GameServerInfo server)
        {
            // we check private port here
            if (server.ServerData.KeyValue.ContainsKey("privateport"))
            {
                if (server.ServerData.KeyValue["privateport"] != "")
                {
                    header[0] ^= (byte)GameServerFlags.NonStandardPrivatePortFlag;
                    byte[] port = HtonsExtensions.PortToIntBytes(server.ServerData.KeyValue["privateport"]);
                    header.AddRange(port);
                }
            }
        }

        protected void CheckICMPSupport(List<byte> header, GameServerInfo server)
        {
            header[0] ^= (byte)GameServerFlags.ICMPIPFlag;
            byte[] address = HtonsExtensions.IPStringToBytes(server.RemoteQueryReportIP);
            header.AddRange(address);
        }

        protected void BuildUniqueValue()
        {
            //because we are using NTS string so we do not have any value here
            _serverListContext.Add(0);
        }

        protected void BuildServerKeys()
        {
            //we add the total number of the requested keys
            _serverListContext.Add((byte)_request.Keys.Length);
            //then we add the keys
            foreach (var key in _request.Keys)
            {
                _serverListContext.Add((byte)SBKeyType.String);
                _serverListContext.AddRange(Encoding.ASCII.GetBytes(key));
                _serverListContext.Add(SBStringFlag.StringSpliter);
            }
        }
    }
}