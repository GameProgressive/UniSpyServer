using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Request.General;
using Chat.Entity.Structure.Result.General;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.General
{
    internal sealed class WHOResponse : ChatResponseBase
    {
        private new WHORequest _request => (WHORequest)base._request;
        private new WHOResult _result => (WHOResult)base._result;
        public WHOResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        protected override void BuildNormalResponse()
        {
            SendingBuffer = "";
            foreach (var data in _result.DataModels)
            {
                SendingBuffer += BuildWhoReply(data);
            }
            switch (_request.RequestType)
            {
                case WHOType.GetChannelUsersInfo:
                    SendingBuffer += BuildEndOfWhoReply(_result.DataModels[0].ChannelName);
                    break;
                case WHOType.GetUserInfo:
                    SendingBuffer += BuildEndOfWhoReply(_result.DataModels[0].NickName);
                    break;
            }
        }
        public static string BuildWhoReply(WHODataModel data)
        {
            var cmdParams =
                $"param1 {data.ChannelName} {data.UserName} " +
                $"{data.PublicIPAddress} param5 {data.NickName} {data.Modes} param8";
            return ChatIRCReplyBuilder.Build(ChatReplyName.WhoReply, cmdParams);
        }

        public static string BuildEndOfWhoReply(string name)
        {
            var cmdParams = $"param1 {name} param3";
            var tailing = "End of WHO.";
            return ChatIRCReplyBuilder.Build(
               ChatReplyName.EndOfWho,
               cmdParams,
               tailing);
        }


    }
}

