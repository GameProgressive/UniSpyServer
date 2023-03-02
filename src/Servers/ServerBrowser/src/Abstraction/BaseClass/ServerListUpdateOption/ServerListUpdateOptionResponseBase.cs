using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UniSpy.Server.ServerBrowser.Application;
using UniSpy.Server.ServerBrowser.Enumerate;
using UniSpy.Server.ServerBrowser.Aggregate.Misc;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.QueryReport.Aggregate.Redis.GameServer;

namespace UniSpy.Server.ServerBrowser.Abstraction.BaseClass
{
    public abstract class ServerListUpdateOptionResponseBase : ResponseBase
    {
        protected new ServerListUpdateOptionRequestBase _request => (ServerListUpdateOptionRequestBase)base._request;
        protected new ServerListUpdateOptionResultBase _result => (ServerListUpdateOptionResultBase)base._result;
        protected List<byte> _serverListData;
        public ServerListUpdateOptionResponseBase(RequestBase request, ResultBase result) : base(request, result)
        {
            _serverListData = new List<byte>();
        }

        public override void Build()
        {
            //todo check protocol version to build response data
            //Add crypt header
            BuildCryptHeader();
            _serverListData.AddRange(_result.ClientRemoteIP);
            _serverListData.AddRange(ClientInfo.HtonQueryReportDefaultPort);
        }
        protected void BuildCryptHeader()
        {
            // cryptHeader have 14 bytes, when we encrypt data we need skip the first 14 bytes
            var cryptHeader = new List<byte>();
            cryptHeader.Add(2 ^ 0xEC);
            #region message length?
            cryptHeader.AddRange(new byte[] { 0, 0 });
            #endregion
            cryptHeader.Add((byte)(ClientInfo.ServerChallenge.Length ^ 0xEA));
            cryptHeader.AddRange(UniSpyEncoding.GetBytes(ClientInfo.ServerChallenge));
            _serverListData.AddRange(cryptHeader);
        }
        protected abstract void BuildServerFullInfo();
        /// <summary>
        /// Add more server info here
        /// the sequence of server info is important
        /// 1.PRIVATE_IP_FLAG length=4
        /// 2.ICMP_IP_FLAG length=4
        /// 3.NONSTANDARD_PORT_FLAG length=2
        /// 4.NONSTANDARD_PRIVATE_PORT_FLAG length=2
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="serverInfo"></param>
        protected void BuildServerInfoHeader(GameServerFlags? flag, GameServerInfo serverInfo)
        {
            List<byte> header = new List<byte>();
            // add key flag
            header.Add((byte)flag);
            // we add server public ip here
            header.AddRange(serverInfo.HostIPAddress.GetAddressBytes());
            // we check host port is standard port or not
            CheckNonStandardPort(header, serverInfo);
            // check if game can directly query information from server
            CheckUnsolicitedUdp(header, serverInfo);
            // we check the natneg flag
            CheckNatNegFlag(header, serverInfo);
            // now we check if there are private ip
            CheckPrivateIP(header, serverInfo);
            // we check private port here
            CheckNonStandardPrivatePort(header, serverInfo);
            // we check icmp support here
            CheckICMPSupport(header, serverInfo);

            _serverListData.AddRange(header);
        }
        protected void CheckNatNegFlag(List<byte> header, GameServerInfo serverInfo)
        {
            if (serverInfo.ServerData.ContainsKey("natneg"))
            {
                var natNegFlag = int.Parse(serverInfo.ServerData["natneg"]);
                var unsolicitedUdp = header[0] & (byte)GameServerFlags.UnsolicitedUdpFlag;
                if (natNegFlag == 1 && unsolicitedUdp == 0)
                {
                    header[0] ^= (byte)GameServerFlags.ConnectNegotiateFlag;
                }
            }
        }
        protected void CheckUnsolicitedUdp(List<byte> header, GameServerInfo serverInfo)
        {
            if (serverInfo.ServerData.ContainsKey("allow_unsolicited_udp"))
            {
                var unsolicitedUdp = int.Parse(serverInfo.ServerData["unsolicitedudp"]);
                if (unsolicitedUdp == 1)
                {
                    header[0] ^= (byte)GameServerFlags.UnsolicitedUdpFlag;
                }
            }
        }
        /// <summary>
        /// when game create a channel chat, it will use the public ip and private ip to build the name.
        /// Known game: Worm3d
        /// </summary>
        /// <param name="header"></param>
        /// <param name="serverInfo"></param>
        protected void CheckPrivateIP(List<byte> header, GameServerInfo serverInfo)
        {
            var privateFlagGame = new List<string>() { "Worm3d" };
            if (!privateFlagGame.Contains(_request.GameName))
            {
                return;
            }
            // We already have the localip. Bytes are worng.
            if (serverInfo.ServerData.ContainsKey("localip0"))
            {
                header[0] ^= (byte)GameServerFlags.PrivateIPFlag;
                // there are multiple localip in dictionary we do not know which one is needed here,
                // so we just send the first one.
                byte[] bytesAddress = IPAddress.Parse(serverInfo.ServerData["localip0"]).GetAddressBytes();
                header.AddRange(bytesAddress);
            }
        }
        protected void CheckNonStandardPort(List<byte> header, GameServerInfo serverInfo)
        {
            // !! only dedicated server have different query report port and host port
            // !! but peer server have same query report port and host port
            // todo we have to check when we need send host port or query report port
            if (serverInfo.QueryReportPort != ClientInfo.QueryReportDefaultPort)
            {
                header[0] ^= (byte)GameServerFlags.NonStandardPort;
                byte[] htonPort = BitConverter.GetBytes((ushort)serverInfo.QueryReportPort).Reverse().ToArray();
                header.AddRange(htonPort);
            }
        }
        protected void CheckNonStandardPrivatePort(List<byte> header, GameServerInfo serverInfo)
        {
            // we check private port here
            if (serverInfo.ServerData.ContainsKey("localport"))
            {
                if (serverInfo.ServerData["localport"] != ""
                && serverInfo.ServerData["localport"] != ClientInfo.QueryReportDefaultPort.ToString())
                {
                    header[0] ^= (byte)GameServerFlags.NonStandardPrivatePortFlag;
                    byte[] port = BitConverter.GetBytes(short.Parse(serverInfo.ServerData["localport"]));
                    header.AddRange(port);
                }
            }
        }
        protected void CheckICMPSupport(List<byte> header, GameServerInfo serverInfo)
        {
            if (serverInfo.ServerData.ContainsKey("icmp_address"))
            {
                header[0] ^= (byte)GameServerFlags.IcmpIpFlag;
                byte[] bytesAddress = IPAddress.Parse(serverInfo.ServerData["icmp_address"]).GetAddressBytes();
                header.AddRange(bytesAddress);
            }
        }
        protected void BuildUniqueValue()
        {
            //because we are using NTS string so we do not have any value here
            _serverListData.Add(0);
        }
        protected void BuildServerKeys()
        {
            //we add the total number of the requested keys
            _serverListData.Add((byte)_request.Keys.Length);
            //then we add the keys
            foreach (var key in _request.Keys)
            {
                _serverListData.Add((byte)DataKeyType.String);
                _serverListData.AddRange(UniSpyEncoding.GetBytes(key));
                _serverListData.Add(StringFlag.StringSpliter);
            }
        }
    }
}
