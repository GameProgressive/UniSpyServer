using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Contract.Request.General;

namespace UniSpy.Server.Chat.Handler.CmdHandler.General
{
    /// <summary>
    /// set global key value on this client
    /// </summary>
    public sealed class SetKeyHandler : LogedInHandlerBase
    {
        private new SetKeyRequest _request => (SetKeyRequest)base._request;
        public SetKeyHandler(IChatClient client, SetKeyRequest request) : base(client, request) { }

        protected override void DataOperation()
        {
            _client.Info.KeyValues.Update(_request.KeyValues);
            // foreach (var channel in _client.Info.JoinedChannels.Values)
            // {
            //     ChannelUser user = channel.GetChannelUser(_client);
            //     //if (user==null)
            //     //{
            //     //    continue;
            //     //}
            //     user.UpdateUserKeyValues(_request.KeyValues);
            // }
        }
    }
}
