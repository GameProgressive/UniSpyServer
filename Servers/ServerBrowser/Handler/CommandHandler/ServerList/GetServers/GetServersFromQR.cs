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
            //the key lenth
            _totalKeysNumber = keys.Length;
            data.Add((byte)keys.Length);

            //The following byte should be keyType: maybe serverkey playerkey teamkey
            foreach (var field in keys)
            {
                //get every keys key type
                data.Add((byte)SBKeyType.String);
                data.AddRange(Encoding.ASCII.GetBytes(field));
                data.Add(0);
            }
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
                    string temp = server.Value.Server.StandardKeyValue[key];
                    //data.Add(Convert.ToByte(temp.Length));
                    data.AddRange(Encoding.ASCII.GetBytes(temp));
                    data.Add(0); // Field Seperator
                }
            }
            return data.ToArray();
        }

        public byte[] GenerateServerInfos()
        {
            List<byte> data = new List<byte>();

            foreach (var server in _filteredServers)
            {
                data.AddRange(GenerateInfoHeader(server));

                //We have to generate unique value string index
                for (int valueIndex = 0; valueIndex < _totalKeysNumber; valueIndex++)
                {
                    data.Add((byte)valueIndex);
                }
            }
            return data.ToArray();
        }

        private byte[] GenerateInfoHeader(KeyValuePair<EndPoint, GameServer> server)
        {
            // you will only have HasKeysFlag or HasFullRule you can not have both
            List<byte> infoHeader = new List<byte>();

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
            if (server.Value.Server.StandardKeyValue.ContainsKey("localip0"))
            {
                infoHeader[0] ^= (byte)GameServerFlags.PrivateIPFlag;
                var address = IPAddress.Parse(server.Value.Server.StandardKeyValue["localip0"]);
                address.GetAddressBytes();
                infoHeader.AddRange(address.GetAddressBytes());
            }
        }

        private void CheckNonStandardPort(List<byte> infoHeader, KeyValuePair<EndPoint, GameServer> server)
        {
            //we check host port is standard port or not
            if (server.Value.Server.StandardKeyValue.ContainsKey("hostport"))
            {
                if (server.Value.Server.StandardKeyValue["hostport"] != "6500")
                {
                    infoHeader[0] ^= (byte)GameServerFlags.NonStandardPort;
                    byte[] port = BitConverter.GetBytes(
                        ushort.Parse(server.Value.Server.StandardKeyValue["hostport"]));
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
            if (server.Value.Server.StandardKeyValue.ContainsKey("localport"))
            {
                infoHeader[0] ^= (byte)GameServerFlags.NonStandardPrivatePortFlag;
                byte[] localport = BitConverter.GetBytes(ushort.Parse(server.Value.Server.StandardKeyValue["localport"]));
                infoHeader.AddRange(localport);
            }
        }
    }
}
