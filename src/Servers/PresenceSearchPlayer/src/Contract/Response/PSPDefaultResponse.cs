using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;

namespace UniSpy.Server.PresenceSearchPlayer.Contract.Response
{
    public sealed class PSPDefaultResponse : ResponseBase
    {
        public PSPDefaultResponse(RequestBase request, UniSpy.Server.Core.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
        }
    }
}
