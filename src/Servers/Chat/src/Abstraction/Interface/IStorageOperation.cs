using System.Net;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis.GameServer;

namespace UniSpyServer.Servers.Chat.Abstraction.Interface
{
    public interface IStorageOperation
    {
        (int userId, int profileId, bool emailVerified, bool banned) NickAndEmailLogin(string nickName, string email, string passwordHash);
        (int userId, int profileId, bool emailVerified, bool banned) UniqueNickLogin(string uniqueNick, int namespaceId);
        bool IsPeerLobby(string channelName);
        void DeleteGameServerInfo(IPAddress address, string gameName);
    }
}