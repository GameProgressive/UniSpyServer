using System;
using System.Collections.Generic;
using System.Net;
using UniSpy.Server.QueryReport.V2.Aggregate.Redis.GameServer;
using UniSpy.Server.ServerBrowser.V2.Application;
using UniSpy.Server.ServerBrowser.V2.Enumerate;

namespace UniSpy.Server.ServerBrowser.V2.Aggregate
{
    public static class ServerInfoBuilder
    {

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
        public static List<byte> BuildServerInfoHeader(GameServerFlags flag, GameServerInfo serverInfo)
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

            // _serverListData.AddRange(header);
            return header;
        }
        public static void CheckNatNegFlag(List<byte> header, GameServerInfo serverInfo)
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
        public static void CheckUnsolicitedUdp(List<byte> header, GameServerInfo serverInfo)
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
        /// !when game create a channel chat, it will use both the public ip and private ip to build the name.
        /// !Known game: Worm3d
        /// </summary>
        public static void CheckPrivateIP(List<byte> header, GameServerInfo serverInfo)
        {
            if (QueryReport.Application.StorageOperation.PeerGroupList.ContainsKey(serverInfo.GameName))
            {
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
        }
        public static void CheckNonStandardPort(List<byte> header, GameServerInfo serverInfo)
        {
            // !! only dedicated server have different query report port and host port
            // !! but peer server have same query report port and host port
            // todo we have to check when we need send host port or query report port
            if (serverInfo.QueryReportPort != ClientInfo.QueryReportDefaultPort)
            {
                header[0] ^= (byte)GameServerFlags.NonStandardPort;
                byte[] htonPort = serverInfo.QueryReportPortBytes;
                header.AddRange(htonPort);
            }
        }
        public static void CheckNonStandardPrivatePort(List<byte> header, GameServerInfo serverInfo)
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
        public static void CheckICMPSupport(List<byte> header, GameServerInfo serverInfo)
        {
            if (serverInfo.ServerData.ContainsKey("icmp_address"))
            {
                header[0] ^= (byte)GameServerFlags.IcmpIpFlag;
                byte[] bytesAddress = IPAddress.Parse(serverInfo.ServerData["icmp_address"]).GetAddressBytes();
                header.AddRange(bytesAddress);
            }
        }

    }
}