using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc.ChannelInfo;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.General
{
    /// <summary>
    /// set every channel key value on this user
    /// </summary>
    
    public sealed class SetKeyHandler : LogedInHandlerBase
    {
        private new SetKeyRequest _request => (SetKeyRequest)base._request;
        public SetKeyHandler(IClient client, IRequest request) : base(client, request){ }

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
