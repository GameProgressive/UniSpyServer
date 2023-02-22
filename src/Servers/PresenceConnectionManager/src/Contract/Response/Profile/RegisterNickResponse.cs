using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;

namespace UniSpy.Server.PresenceConnectionManager.Contract.Response
{
    public sealed class RegisterNickResponse : ResponseBase
    {
        public RegisterNickResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\rn\\id\{_request.OperationID}\final\";
        }
    }
}
