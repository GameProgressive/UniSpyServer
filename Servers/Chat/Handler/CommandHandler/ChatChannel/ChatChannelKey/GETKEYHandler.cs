using System;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    /// <summary>
    /// Get value of the channel user's key value of all channels
    /// </summary>
    public class GETKEYHandler : ChatLogedInHandlerBase
    {
        new GETKEY _cmd;
        public GETKEYHandler(ISession session, ChatCommandBase cmd) : base(session, cmd)
        {
            _cmd = (GETKEY)cmd;
        }

        public override void DataOperation()
        {
            base.DataOperation();
            _sendingBuffer = "";
            foreach (var channel in _session.UserInfo.JoinedChannels)
            {
                ChatChannelUser user;
                if (channel.GetChannelUserBySession(_session, out user))
                {
                    string valueStr = user.GetUserValuesString(_cmd.Keys);
                    _sendingBuffer += ChatReply.BuildGetKeyReply(_session.UserInfo, _cmd.Cookie, valueStr);
                }
            }
            _sendingBuffer += ChatReply.BuildEndOfGetKeyReply(_session.UserInfo, _cmd.Cookie);
        }
    }
}
