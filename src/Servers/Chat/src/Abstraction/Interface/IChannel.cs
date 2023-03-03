using System.Collections.Generic;
using UniSpy.Server.Chat.Aggregate.Misc.ChannelInfo;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Abstraction.Interface
{
    public interface IChannel
    {
        void MultiCast(IClient sender, IResponse message, bool isSkipSender);
        // void MultiCastExceptSender(ChannelUser sender, IResponse message);
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