using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;

namespace UniSpy.Server.PresenceConnectionManager.Contract.Response
{
    public sealed class RegisterCDKeyResponse : ResponseBase
    {
        public RegisterCDKeyResponse(UniSpy.Server.Core.Abstraction.BaseClass.RequestBase request, UniSpy.Server.Core.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = @"\rc\\final\";
        }
    }
}
