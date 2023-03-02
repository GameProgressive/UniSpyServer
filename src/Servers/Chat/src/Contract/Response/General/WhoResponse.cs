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
        public WhoResponse(RequestBase request, ResultBase result) : base(request, result){ }
        public override void Build()
        {
            SendingBuffer = "";
            foreach (var data in _result.DataModels)
            {
                SendingBuffer += BuildWhoReply(data);
            }
            switch (_request.RequestType)
            {
                case WhoRequestType.GetChannelUsersInfo:
                    SendingBuffer += BuildEndOfWhoReply(_result.DataModels[0].ChannelName);
                    break;
                case WhoRequestType.GetUserInfo:
                    SendingBuffer += BuildEndOfWhoReply(_result.DataModels[0].NickName);
                    break;
            }
        }
        public static string BuildWhoReply(WhoDataModel data)
        {
            var cmdParams =
                $"param1 {data.ChannelName} {data.UserName} " +
                $"{data.PublicIPAddress} param5 {data.NickName} {data.Modes} param8";
            return IRCReplyBuilder.Build(ResponseName.WhoReply, cmdParams);
        }

        public static string BuildEndOfWhoReply(string name)
        {
            var cmdParams = $"param1 {name} param3";
            var tailing = "End of WHO.";
            return IRCReplyBuilder.Build(
               ResponseName.EndOfWho,
               cmdParams,
               tailing);
        }


    }
}
