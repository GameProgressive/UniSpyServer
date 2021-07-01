using PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Response
{
    internal sealed class RegisterCDKeyResponse : PCMResponseBase
    {
        public RegisterCDKeyResponse(UniSpyRequest request, UniSpyResult result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = @"\rc\\final\";
        }
    }
}
