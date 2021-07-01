using Chat.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response
{
    internal sealed class ChatDefaultResponse : ChatResponseBase
    {
        public ChatDefaultResponse(UniSpyRequest request, UniSpyResult result) : base(request, result)
        {
        }

        public override void Build()
        {
        }
    }
}
