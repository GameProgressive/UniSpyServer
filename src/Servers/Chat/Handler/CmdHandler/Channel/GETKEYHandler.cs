using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.Response.Channel;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.ChatChannelCmdHandler
{
    /// <summary>
    /// Get value of the channel user's key value of all channels
    /// </summary>
    public class GETKEYHandler : ChatLogedInHandlerBase
    {
        protected new GETKEYRequest _request
        {
            get { return (GETKEYRequest)base._request; }
        }
        public GETKEYHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
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
