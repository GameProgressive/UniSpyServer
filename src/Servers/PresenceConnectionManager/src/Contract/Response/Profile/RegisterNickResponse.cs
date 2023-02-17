using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;

namespace UniSpy.Server.PresenceConnectionManager.Contract.Response
{
    public sealed class RegisterNickResponse : ResponseBase
    {
        public RegisterNickResponse(UniSpy.Server.Core.Abstraction.BaseClass.RequestBase request, UniSpy.Server.Core.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\rn\\id\{_request.OperationID}\final\";
        }
    }
}
