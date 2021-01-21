using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Request.Channel;
using Chat.Entity.Structure.Response.Channel;
using Chat.Entity.Structure.Result;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Channel
{
    // Sets channel key/values.
    // If user is NULL or "", the keys will be set on the channel.
    // Otherwise, they will be set on the user,
    // Only ops can set channel keys on other users.
    // Set a value to NULL or "" to clear that key.
    internal class GETCHANKEYHandler : ChatChannelHandlerBase
    {
        protected new GETCHANKEYRequest _request => (GETCHANKEYRequest)base._request;
        protected new GETCHANKEYResult _result
        {
            get => (GETCHANKEYResult)base._result;
            set => base._result = value;
        }
        public GETCHANKEYHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new GETCHANKEYResult();
        }

        protected override void DataOperation()
        {

            _result.Values = _channel.Property.GetChannelValueString(_request.Keys);
            _result.ChannelUser = _user;
            _result.ChannelName = _channel.Property.ChannelName;
        }

        protected override void ResponseConstruct()
        {
            _response = new GETCHANKEYResponse(_request, _result);
        }
    }
}
