using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using QueryReport.Entity.Structure;
using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Entity.Interface;
using ServerBrowser.Entity.Structure;
using ServerBrowser.Entity.Structure.Packet.Request;
using ServerBrowser.Handler.CommandHandler.ServerList.GetServers.Filter;

namespace ServerBrowser.Handler.CommandHandler.ServerList.GetServers
{
    public class GetServersFromQR
    {
        public static IEnumerable<KeyValuePair<EndPoint, GameServer>> GetFilteredServer(IGetServerable iServer, string gameName, string filter)
        {
            return ServerFilter.GetFilteredServer(iServer.GetOnlineServers(gameName), filter);
        }

        //private List<byte> GenerateServerKeys(ServerListRequest _request)
        //{
        //    List<byte> data = new List<byte>();

        //    switch (_request.UpdateOption)
        //    {
        //        case SBServerListUpdateOption.NoServerList:
        //            //we do not need to add keys
        //            data.Add(0);
        //            break;
        //        case SBServerListUpdateOption.SendRequestedField:
        //            data.Add((byte)_request.FieldList.Length);
        //            foreach (var key in _request.FieldList)
        //            {
        //                data.Add((byte)SBKeyType.String);
        //                data.AddRange(Encoding.ASCII.GetBytes(key));
        //                data.Add(0);
        //            }
        //            break;
        //    }
        //    return data;
        //}
        //private List<byte> GenerateUniqueValue(ServerListRequest _request,bool _IsUsingNTSValue)
        //{

        //    List<byte> data = new List<byte>();
        //    switch (_request.UpdateOption)
        //    {
        //        case SBServerListUpdateOption.NoServerList:
        //            break;
        //        case SBServerListUpdateOption.SendRequestedField:
        //            if (_IsUsingNTSValue)
        //            {
        //                //we do not add unique value here
        //                //because we are using NTS string
        //                data.Add(0);
        //            }
        //            else
        //            {
        //                //we add unique value here
        //            }
        //            break;
        //    }

        //    return data;
        //}
        //private List<byte> GenerateServersInfo(ServerListRequest _request, IEnumerable<KeyValuePair<EndPoint, GameServer>> _filteredServers,bool _IsUsingNTSValue)
        //{
        //    List<byte> data = new List<byte>();

        //    foreach (var server in _filteredServers)
        //    {
        //        data.AddRange(GenerateServerInfoHeader(server));

        //        switch (_request.UpdateOption)
        //        {
        //            case SBServerListUpdateOption.SendRequestedField:
        //                //add every value to list
        //                if (_IsUsingNTSValue)
        //                {
        //                    foreach (var key in _request.FieldList)
        //                    {
        //                        data.Add(SBStringFlag.NTSStringFlag);
        //                        data.AddRange(Encoding.ASCII.GetBytes(server.Value.ServerData.StandardKeyValue[key]));
        //                        data.Add(SBStringFlag.StringSpliter);
        //                    }
        //                }
        //                else
        //                {
        //                    //do unique values method
        //                }
        //                break;
        //            case SBServerListUpdateOption.NoServerList:
        //                break;
        //        }
        //    }
        //    return data;
        //}

        //private List<byte> GenerateServerInfoHeader(KeyValuePair<EndPoint, GameServer> server)
        //{
        //    // you will only have HasKeysFlag or HasFullRule you can not have both
        //    List<byte> header = new List<byte>();

        //    //add has key flag
        //    header.Add((byte)GameServerFlags.HasKeysFlag);

        //    //we add server public ip here
        //    header.AddRange(BitConverter.GetBytes(server.Value.PublicIP));

        //    //we check host port is standard port or not
        //    CheckNonStandardPort(header, server);

        //    // now we check if there are private ip
        //    CheckPrivateIP(header, server);

        //    // we check private port here
        //    CheckPrivatePort(header, server);

        //    //TODO we have to check icmp_ip_flag

        //    return header;
        //}
        //private void CheckPrivateIP(List<byte> header, KeyValuePair<EndPoint, GameServer> server)
        //{
        //    // now we check if there are private ip
        //    if (server.Value.ServerData.StandardKeyValue.ContainsKey("localip0"))
        //    {
        //        header[0] ^= (byte)GameServerFlags.PrivateIPFlag;
        //        byte[] address = IPAddress.Parse(server.Value.ServerData.StandardKeyValue["localip0"]).GetAddressBytes();
        //        header.AddRange(address);
        //    }
        //    else if (server.Value.ServerData.StandardKeyValue.ContainsKey("localip1"))
        //    {
        //        header[0] ^= (byte)GameServerFlags.PrivateIPFlag;
        //        byte[] address = IPAddress.Parse(server.Value.ServerData.StandardKeyValue["localip1"]).GetAddressBytes();
        //        header.AddRange(address);
        //    }
        //}
        //private void CheckNonStandardPort(List<byte> header, KeyValuePair<EndPoint, GameServer> server)
        //{
        //    //we check host port is standard port or not
        //    if (server.Value.ServerData.StandardKeyValue.ContainsKey("hostport"))
        //    {
        //        if (server.Value.ServerData.StandardKeyValue["hostport"] != "6500")
        //        {
        //            header[0] ^= (byte)GameServerFlags.NonStandardPort;
        //            //we do not need to reverse port bytes
        //            byte[] port = BitConverter.GetBytes(ushort.Parse(server.Value.ServerData.StandardKeyValue["hostport"]));

        //            header.AddRange(port);
        //        }
        //    }
        //}
        //private void CheckPrivatePort(List<byte> header, KeyValuePair<EndPoint, GameServer> server)
        //{
        //    // we check private port here
        //    if (server.Value.ServerData.StandardKeyValue.ContainsKey("localport"))
        //    {
        //        header[0] ^= (byte)GameServerFlags.NonStandardPrivatePortFlag;
        //        byte[] localPort =
        //         BitConverter.GetBytes(ushort.Parse(server.Value.ServerData.StandardKeyValue["localport"]));

        //        header.AddRange(localPort);
        //    }
        //}
    }
}
