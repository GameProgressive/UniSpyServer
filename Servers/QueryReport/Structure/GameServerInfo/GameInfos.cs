using System;
using System.Collections.Generic;
using System.Text;

namespace QueryReport.GameServerInfo
{
    public class GameInfo
    {
        public int ID;
        public string Name;
        public string SecretKey;
        public ushort QueryPort;
        public ushort BackendFlags;
        public uint ServicesDisabled;
        public KeyData KeyData = new KeyData();
        public byte NumPushKeys;

        public void SetGameInfo(byte[] recv)
        {

        }

    }

    public class KeyData
    {
        public string Name;
        public byte Type;

    }

    public class ModInfo
    {
        public byte[] Name = new byte[128];
        public byte[] Description = new byte[128];
    }

    public class CustomKey
    {
        public string Name;
        public string Value;
    }

    public class IndexedKey
    {
        public CustomKey CustomKey = new CustomKey();
        public int index;
    }
    public class CountryRegion
    {
        public string CountryCode;
        public string CountryName;
        public uint Region;
    }

    public class ServerList
    {
        public List<CustomKey> ServerKeys = new List<CustomKey>();
        public CountryRegion Country = new CountryRegion();
        public uint IPaddress;
        public ushort Port;
    }

    public class QRServerList
    {
        public GameInfo Game= new GameInfo();
        public byte Filter;
        public List<ServerList> ServerList = new List<ServerList>();
        public int NumServers;
    }
    public class QRServerRules
    {
        public GameInfo Game = new GameInfo();
        public uint IPaddress;
        public ushort Port;
        public CountryRegion Country = new CountryRegion();
        public List<CustomKey> ServerRules = new List<CustomKey>();
    }
    public class QRClientMsg
    {
        public string Data;
        public int Len;
        public uint SourceIP;
        public uint ToIP;
        public ushort ToPort;
    }
    public class QRServerMsg
    {
        public byte MsgID;
        public string Data;
    }
}
