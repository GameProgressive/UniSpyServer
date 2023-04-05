using UniSpy.Server.ServerBrowser.V1.Abstraction.BaseClass;

namespace UniSpy.Server.ServerBrowser.V1.Contract.Response
{
    public sealed class GameNameResponse : ResponseBase
    {
        /// <summary>
        /// Crypt key is 14 char long string
        /// </summary>
        public static string CryptKey = "00000000000000";
        public GameNameResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            throw new System.NotImplementedException();
        }
    }
}