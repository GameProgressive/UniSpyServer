using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler
{
    [HandlerContract("registercdkey")]
    public sealed class RegisterCDKeyHandler : Abstraction.BaseClass.CmdHandlerBase
    {
        private new RegisterCDKeyRequest _request => (RegisterCDKeyRequest)base._request;
        public RegisterCDKeyHandler(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void DataOperation()
        {
            using (var db = new UniSpyContext())
            {
                var result = db.Subprofiles.Where(s => s.ProfileId == _client.Info.BasicInfo.ProfileId
                && s.Namespaceid == _client.Info.BasicInfo.NamespaceId);
                //&& s.Productid == _client.Info.ProductID);

                if (result.Count() == 0 || result.Count() > 1)
                {
                    throw new GPDatabaseException("No user infomation found in database.");
                }

                db.Subprofiles.Where(s => s.Subprofileid == _client.Info.BasicInfo.SubProfileId)
                    .FirstOrDefault().Cdkeyenc = _request.CDKeyEnc;

                db.SaveChanges();
            }
        }
    }
}
