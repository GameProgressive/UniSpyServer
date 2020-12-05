using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CommandHandler.ChatChannelCommandHandler
{
    /// <summary>
    /// set every channel key value on this user
    /// </summary>
    public class SETKEYHandler : ChatLogedInHandlerBase
    {
        new SETKEYRequest _request;
        public SETKEYHandler(IUniSpySession session, ChatRequestBase request) : base(session, request)
        {
            _request = (SETKEYRequest)request;
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            //string buffer = ChatReply.BuildGetKeyReply()
            foreach (var channel in _session.UserInfo.JoinedChannels)
            {
                ChatChannelUser user;
                if (channel.GetChannelUserBySession(_session, out user))
                {
                    user.UpdateUserKeyValue(_request.KeyValues);
                }
            }
        }
    }
}
