using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Result.General;

namespace UniSpy.Server.Chat.Contract.Response.General
{
    public sealed class WhoResponse : ResponseBase
    {
        private new WhoRequest _request => (WhoRequest)base._request;
        private new WhoResult _result => (WhoResult)base._result;
        public WhoResponse(RequestBase request, ResultBase result) : base(request, result) { }
        public override void Build()
        {
            SendingBuffer = "";
            foreach (var data in _result.DataModels)
            {
                SendingBuffer += $":{ServerDomain} {ResponseName.WhoReply} * {data.ChannelName} {data.UserName} {data.PublicIPAddress} * {data.NickName} {data.Modes} *\r\n";
            }
            switch (_request.RequestType)
            {
                case WhoRequestType.GetChannelUsersInfo:
                    if (_result.DataModels.Count > 0)
                    {
                        SendingBuffer += $":{ServerDomain} {ResponseName.EndOfWho} * {_request.ChannelName} * :End of WHO.\r\n";
                    }
                    break;
                case WhoRequestType.GetUserInfo:
                    if (_result.DataModels.Count > 0)
                    {
                        SendingBuffer += $":{ServerDomain} {ResponseName.EndOfWho} * {_request.NickName} * :End of WHO.\r\n";
                    }
                    break;
            }
        }
    }
}

