using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatResponse.ChatGeneralResponse
{
    public class CRYPTReply
    {
        public static string BuildCryptReply(string clientKey, string serverKey)
        {
            return ChatReplyBase.BuildReply(
                    ChatReplyCode.SecureKey,
                    $"* {clientKey} {serverKey}");
        }
    }
}
