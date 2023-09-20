using UniSpy.Server.ServerBrowser.V1.Abstraction.BaseClass;

namespace UniSpy.Server.ServerBrowser.V1.Contract.Response
{
    public sealed class GameNameResponse : ResponseBase
    {
        public GameNameResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            throw new System.NotImplementedException();
        }
    }
}