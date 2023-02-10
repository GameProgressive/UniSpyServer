using System.Collections.Generic;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc.ChannelInfo;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Abstraction.Interface
{
    public interface IChannel
    {
        void MultiCast(IClient sender, IResponse message);
        void MultiCastExceptSender(ChannelUser sender, IResponse message);
        string GetAllUsersNickString();
        void AddBindOnUserAndChannel(ChannelUser joiner);
        void RemoveBindOnUserAndChannel(ChannelUser leaver);
        ChannelUser GetChannelUser(IClient client);
        bool IsUserBanned(ChannelUser user);
        void SetProperties(ChannelUser changer, ModeRequest request);
        void SetChannelKeyValue(Dictionary<string, string> keyValue);
        string GetChannelValueString(List<string> keys);
    }
}