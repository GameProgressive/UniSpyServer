using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc.ChannelInfo;
using Chat.Entity.Structure.Request;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.General
{
    /// <summary>
    /// set every channel key value on this user
    /// </summary>
    internal sealed class SETKEYHandler : ChatLogedInHandlerBase
    {
        new SETKEYRequest _request { get { return (SETKEYRequest)base._request; } }
        public SETKEYHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            foreach (var channel in _session.UserInfo.JoinedChannels)
            {
                ChatChannelUser user = channel.GetChannelUserBySession(_session);
                //if (user==null)
                //{
                //    continue;
                //}
                user.UpdateUserKeyValues(_request.KeyValues);
            }
        }
    }
}
