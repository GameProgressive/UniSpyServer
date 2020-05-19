using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler.ChatChannel.ChatChannelKey
{
    /// <summary>
    /// set every channel key value on this user
    /// </summary>
    public class SETKEYHandler : ChatLogedInHandlerBase
    {
        new SETKEY _cmd;
        public SETKEYHandler(ISession session, ChatCommandBase cmd) : base(session, cmd)
        {
            _cmd = (SETKEY)cmd;
        }

        public override void DataOperation()
        {
            base.DataOperation();
            //string buffer = ChatReply.BuildGetKeyReply()
            foreach (var channel in _session.UserInfo.JoinedChannels)
            {
                ChatChannelUser user;
                if (channel.GetChannelUserBySession(_session, out user))
                {
                    user.UpdateUserKeyValue(_cmd.KeyValues);
                }
            }
        }
    }
}
