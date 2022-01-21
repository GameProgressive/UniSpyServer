// using System.Collections.Generic;

// namespace UniSpyServer.Servers.QueryReport.Entity.Structure
// {
//     public class GameServerInfo
//     {
//         public int ID;
//         public string Name;
//         public string SecretKey;
//         public ushort QueryPort;
//         public ushort BackendFlags;
//         public int ServicesDisabled;
//         public KeyData KeyData = new KeyData();
//         public byte NumPushKeys;

//     }

//     public class KeyData
//     {
//         public string Name;
//         public byte Type;

//     }

//     public class ModInfo
//     {
//         public string Name;
//         public byte[] Description = new byte[128];
//     }

//     public class CustomKey
//     {
//         public string Name;
//         public string Value;
//     }

//     public class IndexedKey
//     {
//         public CustomKey CustomKey = new CustomKey();
//         public int index;
//     }
//     public class CountryRegion
//     {
//         public string CountryCode;
//         public string CountryName;
//         public int Region;
//     }

//     public class ServerList
//     {
//         public List<CustomKey> ServerKeys = new List<CustomKey>();
//         public CountryRegion Country = new CountryRegion();
//         public int IPaddress;
//         public ushort Port;
//     }

//     public class QRServerList
//     {
//         public GameServerInfo Game = new GameServerInfo();
//         public byte Filter;
//         public List<ServerList> ServerList = new List<ServerList>();
//         public int NumServers;
//     }
//     public class QRServerRules
//     {
//         public GameServerInfo Game = new GameServerInfo();
//         public int IPaddress;
//         public ushort Port;
//         public CountryRegion Country = new CountryRegion();
//         public List<CustomKey> ServerRules = new List<CustomKey>();
//     }
//     public class QRClientMsg
//     {
//         public string Data;
//         public int Len;
//         public int SourceIP;
//         public int ToIP;
//         public ushort ToPort;
//     }
//     public class QRServerMsg
//     {
//         public byte MsgID;
//         public string Data;
//     }
// }
