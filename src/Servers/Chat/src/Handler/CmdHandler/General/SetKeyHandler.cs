using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc.ChannelInfo;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.General
{
    /// <summary>
    /// set every channel key value on this user
    /// </summary>
    [HandlerContract("SETKEY")]
    public sealed class SetKeyHandler : LogedInHandlerBase
    {
        private new SetKeyRequest _request => (SetKeyRequest)base._request;
        public SetKeyHandler(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void DataOperation()
        {
            foreach (var channel in _client.Info.JoinedChannels.Values)
            {
                ChannelUser user = channel.GetChannelUser(_client);
                //if (user==null)
                //{
                //    continue;
                //}
                user.UpdateUserKeyValues(_request.KeyValues);
            }
        }
    }
}
