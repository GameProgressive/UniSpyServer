using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Result;

namespace Chat.Entity.Structure.Response.Channel
{
    public class GETKEYResponse : ChatResponseBase
    {
        protected new GETKEYResult _result
        {
            get { return (GETKEYResult)base._result; }
        }

        public GETKEYResponse(GETKEYResult result) : base(result)
        {
        }

        public override void Build()
        {
            SendingBuffer = "";
            foreach (var flag in _result.Flags)
            {
                SendingBuffer += _result.UserInfo.BuildReply(
                   ChatReplyCode.GetKey,
                   $"param1 {_result.UserInfo.NickName} {_result.Cookie} {flag}");
            }

            SendingBuffer += _result.UserInfo
                .BuildReply(ChatReplyCode.EndGetKey, $"param1 param2 {_result.Cookie} param4", "End of GETKEY.");
        }
    }
}
