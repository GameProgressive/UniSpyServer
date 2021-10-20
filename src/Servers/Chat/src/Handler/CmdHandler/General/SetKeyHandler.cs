using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;
using UniSpyServer.Chat.Entity.Structure.Misc.ChannelInfo;
using UniSpyServer.Chat.Entity.Structure.Request;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Chat.Handler.CmdHandler.General
{
    /// <summary>
    /// set every channel key value on this user
    /// </summary>
    [HandlerContract("SETKEY")]
    public sealed class SetKeyHandler : LogedInHandlerBase
    {
        private new SetKeyRequest _request => (SetKeyRequest)base._request;
        public SetKeyHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            foreach (var channel in _session.UserInfo.JoinedChannels)
            {
                ChannelUser user = channel.GetChannelUserBySession(_session);
                //if (user==null)
                //{
                //    continue;
                //}
                user.UpdateUserKeyValues(_request.KeyValues);
            }
        }
    }
}
