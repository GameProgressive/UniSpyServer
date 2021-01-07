using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.ChannelInfo;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.Response.Channel;
using Chat.Entity.Structure.Result;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Channel
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
        protected new GETKEYResult _result
        {
            get { return (GETKEYResult)base._result; }
            set { base._result = value; }
        }
        public GETKEYHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            foreach (var channel in _session.UserInfo.JoinedChannels)
            {
                ChatChannelUser user;
                if (channel.GetChannelUserBySession(_session, out user))
                {
                    var values = user.GetUserValues(_request.Keys);
                    _result = new GETKEYResult(_session.UserInfo, _request.Cookie);
                    _result.Flags.Add(values);
                }
            }
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            _response = new GETKEYResponse(_result);
        }
    }
}
