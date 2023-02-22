using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Database.DatabaseModel;
using System.Linq;

namespace UniSpy.Server.QueryReport.V1.Handler.CmdHandler
{
    public class ValidateHandler : V1.Abstraction.BaseClass.CmdHandlerBase
    {
        public ValidateHandler(IClient client, IRequest request) : base(client, request)
        {
        }
        protected override void DataOperation()
        {
            throw new System.Exception("Crypto not implemented");
            // _client.Crypto = new QRCrypt()
        }
        // todo move to StorageOperation
        private string GetSecretKey(string gameName)
        {
            using (var db = new UniSpyContext())
            {
                var result = from p in db.Games
                             where p.Gamename == gameName
                             select new { p.Secretkey };

                return result.First().Secretkey;
            }
        }
    }
}