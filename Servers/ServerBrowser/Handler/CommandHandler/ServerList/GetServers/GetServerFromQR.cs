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
    public class GetServerFromQR
    {
        IEnumerable<KeyValuePair<EndPoint, GameServer>> _filteredServers;
        int _totalKeysNumber;
        int _totalValueNumber;
        public GetServerFromQR(IGetServerAble iServer,string gameName, string filter)
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
                    string temp = server.Value.ServerKeyValue.Data[key];
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

                //TODO
                data.Add((byte)GameServerFlags.HasKeysFlag); // Server flags !
                data.AddRange(server.Value.RemoteIP);

                //We have to numberd string by its sequence
                for (int uniqueValueIndex = 0; uniqueValueIndex < _totalKeysNumber; uniqueValueIndex++)
                {
                    data.Add((byte)uniqueValueIndex);
                }
            }

            data.Add((byte)GameServerFlags.ServerEnd);
            data.AddRange(new byte[] { 255, 255, 255, 255 });

            return data.ToArray();
        }
    }
}
