using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis.GameServer;
using UniSpyServer.Servers.ServerBrowser.Entity.Enumerate;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Misc;
using UniSpyServer.UniSpyLib.Encryption;

namespace UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass
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
            //add key flag
            header.Add((byte)flag);
            //we add server public ip here
            header.AddRange(serverInfo.HostIPAddress.GetAddressBytes());
            //we check host port is standard port or not
            CheckNonStandardPort(header, serverInfo);
            // now we check if there are private ip
            CheckPrivateIP(header, serverInfo);
            // we check private port here
            CheckNonStandardPrivatePort(header, serverInfo);
            //we check icmp support here
            CheckICMPSupport(header, serverInfo);
            _serverListData.AddRange(header);
        }
        protected void CheckPrivateIP(List<byte> header, GameServerInfo serverInfo)
        {
            // !Fix we do not know how to determine private ip
            List<string> localIPs = serverInfo.ServerData.Where(k => k.Key.Contains("localip") == true).Select(k => k.Value).ToList();
            if (localIPs.Count == 1 & localIPs.First() == serverInfo.HostIPAddress.ToString())
            {
                return;
            }
            else
            {
                header[0] ^= (byte)GameServerFlags.PrivateIPFlag;
                // there are multiple localip in dictionary we do not know which one is needed here,
                // so we just send the first one.
                byte[] bytesAddress = IPAddress.Parse(localIPs.First()).GetAddressBytes();
                header.AddRange(bytesAddress);
            }
        }
        protected void CheckNonStandardPort(List<byte> header, GameServerInfo serverInfo)
        {
            // !! only dedicated server have different query report port and host port
            // !! but peer server have same query report port and host port
            // todo we have to check when we need send host port or query report port

            // if (serverInfo.ServerData.ContainsKey("hostport"))
            // {
            //     if (serverInfo.ServerData["hostport"] != ""
            //         && serverInfo.ServerData["hostport"] != "6500")
            //     {
            //         header[0] ^= (byte)GameServerFlags.NonStandardPort;
            //         byte[] htonPort = BitConverter.GetBytes(ushort.Parse(serverInfo.ServerData["hostport"])).Reverse().ToArray();
            //         header.AddRange(htonPort);
            //     }
            // }
            if (serverInfo.QueryReportPort != 6500)
            {
                header[0] ^= (byte)GameServerFlags.NonStandardPort;
                byte[] htonPort = BitConverter.GetBytes((ushort)serverInfo.QueryReportPort).Reverse().ToArray();
                header.AddRange(htonPort);
            }
        }
        protected void CheckNonStandardPrivatePort(List<byte> header, GameServerInfo serverInfo)
        {
            // we check private port here
            if (serverInfo.ServerData.ContainsKey("privateport"))
            {
                if (serverInfo.ServerData["privateport"] != "")
                {
                    header[0] ^= (byte)GameServerFlags.NonStandardPrivatePortFlag;
                    byte[] port = BitConverter.GetBytes(short.Parse(serverInfo.ServerData["privateport"]));
                    header.AddRange(port);
                }
            }
        }
        protected void CheckICMPSupport(List<byte> header, GameServerInfo serverInfo)
        {
            header[0] ^= (byte)GameServerFlags.ICMPIPFlag;
            byte[] address = serverInfo.HostIPAddress.GetAddressBytes();
            header.AddRange(address);
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
