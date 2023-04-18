using System.Net;

namespace UniSpy.Server.Chat.Abstraction.Interface
{
    public interface IStorageOperation
    {
        (int userId, int profileId, bool emailVerified, bool banned) NickAndEmailLogin(string nickName, string email, string passwordHash);
        (int userId, int profileId, bool emailVerified, bool banned) UniqueNickLogin(string uniqueNick, int namespaceId);
        bool IsPeerLobby(string channelName);
        bool UpdateChannel(Aggregate.Misc.ChannelInfo.Channel channel);
        void RemoveChannel(Aggregate.Misc.ChannelInfo.Channel channel);
        Aggregate.Misc.ChannelInfo.Channel GetChannelInfo(string channelName);
        bool IsChannelExist(string channelName);
        // void DeleteGameServerInfo(IPAddress address, string gameName);
    }
}