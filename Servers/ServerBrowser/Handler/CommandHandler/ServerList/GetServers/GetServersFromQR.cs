using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using QueryReport.Entity.Structure;
using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Entity.Interface;
using ServerBrowser.Handler.CommandHandler.ServerList.GetServers.Filter;

namespace ServerBrowser.Handler.CommandHandler.ServerList.GetServers
{
    public class GetServersFromQR
    {
        IEnumerable<KeyValuePair<EndPoint, GameServer>> _filteredServers;
        int _totalKeysNumber;
        int _totalValueNumber;
        public GetServersFromQR(IGetServerable iServer, string gameName, string filter)
        {
            _filteredServers = new ServerFilter(iServer.GetOnlineServers(gameName), filter).GetFilteredServer();
        }

        public byte[] GenerateServerKeys(string[] keys)
        {
            List<byte> data = new List<byte>();

            _totalKeysNumber = keys.Length;

            data.Add((byte)_totalKeysNumber);

            //The following byte should be keyType: maybe serverkey playerkey teamkey
            foreach (var standardKey in keys)
            {
                //get every keys key type
                data.Add((byte)SBKeyType.String);
                data.AddRange(Encoding.ASCII.GetBytes(standardKey));
                data.Add(0);
            }

            //the key lenth

            return data.ToArray();
        }

        public byte[] GenerateUniqueValues(string[] keys)
        {
            List<byte> data = new List<byte>();
            //this is total value's number include ping value
            _totalValueNumber = _totalKeysNumber * _filteredServers.Count();

            data.Add(Convert.ToByte(_totalValueNumber));

            foreach (var server in _filteredServers)
            {
                foreach (var key in keys)
                {
                    string temp = server.Value.ServerData.StandardKeyValue[key];
                    data.AddRange(Encoding.ASCII.GetBytes(temp));
                    data.Add(0); // Field Seperator
                }

                //fixme we add the server rules here
                foreach (var customKey in server.Value.ServerData.CustomKeyValue)
                {
                    data.AddRange(Encoding.ASCII.GetBytes(customKey.Value));
                    data.Add(0);
                }
            }
            return data.ToArray();
        }

        public byte[] GenerateServerInfos(string[] keys)
        {

            List<byte> data = new List<byte>();

            foreach (var server in _filteredServers)
            {
                data.AddRange(GenerateInfoHeader(server));
                //add every value to list
                foreach (var key in keys)
                {
                    data.Add(0xFF);
                    data.AddRange(Encoding.ASCII.GetBytes(server.Value.ServerData.StandardKeyValue[key]));
                    data.Add(0);
                }

            }
            return data.ToArray();
        }

        private byte[] GenerateInfoHeader(KeyValuePair<EndPoint, GameServer> server)
        {
            // you will only have HasKeysFlag or HasFullRule you can not have both
            List<byte> infoHeader = new List<byte>();

            //add has key flag
            infoHeader.Add((byte)GameServerFlags.HasKeysFlag);

            //we add server public ip here
            infoHeader.AddRange(server.Value.PublicIP);

            // now we check if there are private ip
            CheckPrivateIP(infoHeader, server);

            //TODO we have to check icmp_ip_flag

            //we check host port is standard port or not
            CheckNonStandardPort(infoHeader, server);

            // we check private port here
            CheckPrivatePort(infoHeader, server);

            return infoHeader.ToArray();
        }

        private void CheckPrivateIP(List<byte> infoHeader, KeyValuePair<EndPoint, GameServer> server)
        {
            // now we check if there are private ip
            if (server.Value.ServerData.StandardKeyValue.ContainsKey("localip0"))
            {
                infoHeader[0] ^= (byte)GameServerFlags.PrivateIPFlag;
                byte[] address = IPAddress.Parse(server.Value.ServerData.StandardKeyValue["localip0"]).GetAddressBytes();
                infoHeader.AddRange(address);
            }
        }

        private void CheckNonStandardPort(List<byte> infoHeader, KeyValuePair<EndPoint, GameServer> server)
        {
            //we check host port is standard port or not
            if (server.Value.ServerData.StandardKeyValue.ContainsKey("hostport"))
            {
                if (server.Value.ServerData.StandardKeyValue["hostport"] != "6500")
                {
                    infoHeader[0] ^= (byte)GameServerFlags.NonStandardPort;
                    byte[] port = BitConverter.GetBytes(
                        ushort.Parse(server.Value.ServerData.StandardKeyValue["hostport"]));
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(port);
                    }
                    infoHeader.AddRange(port);
                }
            }
        }
        private void CheckPrivatePort(List<byte> infoHeader, KeyValuePair<EndPoint, GameServer> server)
        {
            // we check private port here
            if (server.Value.ServerData.StandardKeyValue.ContainsKey("localport"))
            {
                infoHeader[0] ^= (byte)GameServerFlags.NonStandardPrivatePortFlag;
                byte[] localPort = BitConverter.GetBytes(ushort.Parse(server.Value.ServerData.StandardKeyValue["localport"]));
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(localPort);
                }
                infoHeader.AddRange(localPort);
            }
        }
    }
}
