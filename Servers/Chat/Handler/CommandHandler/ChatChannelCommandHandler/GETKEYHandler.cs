using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse.ChatChannelResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler.ChatChannelCommandHandler
{
    /// <summary>
    /// Get value of the channel user's key value of all channels
    /// </summary>
    public class GETKEYHandler : ChatLogedInHandlerBase
    {
        new GETKEYRequest _request;
        public GETKEYHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = (GETKEYRequest)request;
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            _sendingBuffer = "";
            foreach (var channel in _session.UserInfo.JoinedChannels)
            {
                ChatChannelUser user;
                if (channel.GetChannelUserBySession(_session, out user))
                {
                    string valueStr = user.GetUserValuesString(_request.Keys);
                    _sendingBuffer += GETKEYReply.BuildGetKeyReply(_session.UserInfo, _request.Cookie, valueStr);
                }
            }
            _sendingBuffer += GETKEYReply.BuildEndOfGetKeyReply(_session.UserInfo, _request.Cookie);
        }
    }
}
