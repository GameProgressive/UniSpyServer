using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;

namespace UniSpy.Server.PresenceConnectionManager.Contract.Response
{
    public sealed class NewProfileResponse : ResponseBase
    {
        public NewProfileResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\npr\\profileid\{SendingBuffer}\id\{_request.OperationID}\final\";
        }
    }
}
