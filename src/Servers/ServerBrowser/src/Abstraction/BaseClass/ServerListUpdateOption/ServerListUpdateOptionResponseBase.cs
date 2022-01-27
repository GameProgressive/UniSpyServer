using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis.GameServer;
using UniSpyServer.Servers.ServerBrowser.Entity.Enumerate;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Misc;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Encryption;

namespace UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass
{
    public abstract class ServerListUpdateOptionResponseBase : ResponseBase
    {
        protected new ServerListUpdateOptionRequestBase _request => (ServerListUpdateOptionRequestBase)base._request;
        protected new ServerListUpdateOptionResultBase _result => (ServerListUpdateOptionResultBase)base._result;
        protected List<byte> _serverListData;
        public ServerListUpdateOptionResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
            _serverListData = new List<byte>();
        }

        public override void Build()
        {
            //todo check protocol version to build response data
            //Add crypt header
            BuildCryptHeader();
            _serverListData.AddRange(_result.ClientRemoteIP);
            _serverListData.AddRange(Constants.HtonQueryReportDefaultPort);
        }
        protected void BuildCryptHeader()
        {
            // cryptHeader have 14 bytes, when we encrypt data we need skip the first 14 bytes
            var cryptHeader = new List<byte>();
            cryptHeader.Add(2 ^ 0xEC);
            #region message length?
            cryptHeader.AddRange(new byte[] { 0, 0 });
            #endregion
            cryptHeader.Add((byte)(Constants.ServerChallenge.Length ^ 0xEA));
            cryptHeader.AddRange(UniSpyEncoding.GetBytes(Constants.ServerChallenge));
            _serverListData.AddRange(cryptHeader);
        }
        protected abstract void BuildServersInfo();
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
            header.AddRange(serverInfo.RemoteIPEndPoint.Address.GetAddressBytes());
            // now we check if there are private ip
            CheckPrivateIP(header, serverInfo);
            //we check icmp support here
            CheckICMPSupport(header, serverInfo);
            //we check host port is standard port or not
            CheckNonStandardPort(header, serverInfo);
            // we check private port here
            CheckNonStandardPrivatePort(header, serverInfo);


            _serverListData.AddRange(header);
        }
        protected void CheckPrivateIP(List<byte> header, GameServerInfo server)
        {
            string localIP = "";
            // now we check if there are private ip
            if (server.ServerData.ContainsKey("localip0"))
            {
                localIP = server.ServerData["localip0"];
            }
            else if (server.ServerData.ContainsKey("localip1"))
            {
                localIP = server.ServerData["localip1"];
            }

            if (localIP != "")
            {
                header[0] ^= (byte)GameServerFlags.PrivateIPFlag;
                byte[] bytesAddress = IPAddress.Parse(localIP).GetAddressBytes();
                header.AddRange(bytesAddress);
            }
        }
        protected void CheckNonStandardPort(List<byte> header, GameServerInfo server)
        {
            ///only dedicated server have different query report port and host port
            ///the query report port and host port are the same on peer server
            ///so we do not need to check this for peer server
            //we check host port is standard port or not
            if (server.ServerData.ContainsKey("hostport"))
            {
                if (server.ServerData["hostport"] != ""
                    && server.ServerData["hostport"] != "6500")
                {
                    header[0] ^= (byte)GameServerFlags.NonStandardPort;
                    byte[] htonPort = BitConverter.GetBytes(short.Parse(server.ServerData["hostport"])).Reverse().ToArray();
                    header.AddRange(htonPort);
                }
            }
        }
        protected void CheckNonStandardPrivatePort(List<byte> header, GameServerInfo server)
        {
            // we check private port here
            if (server.ServerData.ContainsKey("privateport"))
            {
                if (server.ServerData["privateport"] != "")
                {
                    header[0] ^= (byte)GameServerFlags.NonStandardPrivatePortFlag;
                    byte[] port = BitConverter.GetBytes(short.Parse(server.ServerData["privateport"]));
                    header.AddRange(port);
                }
            }
        }
        protected void CheckICMPSupport(List<byte> header, GameServerInfo server)
        {
            header[0] ^= (byte)GameServerFlags.ICMPIPFlag;
            byte[] address = server.RemoteIPEndPoint.Address.GetAddressBytes();
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
