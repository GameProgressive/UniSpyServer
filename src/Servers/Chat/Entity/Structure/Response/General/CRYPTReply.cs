using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.General
{
    public class CRYPTReply
    {
        public static string BuildCryptReply(string clientKey, string serverKey)
        {
            return ChatResponseBase.BuildResponse(
                    ChatReplyCode.SecureKey,
                    $"* {clientKey} {serverKey}");
        }
    }
}
