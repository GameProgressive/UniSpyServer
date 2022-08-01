using System.Linq;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler.Profile
{

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
                var result = db.Subprofiles.Where(s => s.ProfileId == _client.Info.ProfileInfo.ProfileId
                && s.NamespaceId == _client.Info.SubProfileInfo.NamespaceId);
                //&& s.Productid == _client.Info.ProductID);

                if (result.Count() == 0 || result.Count() > 1)
                {
                    throw new GPDatabaseException("No user infomation found in database.");
                }

                db.Subprofiles.FirstOrDefault(s => s.SubProfileId == _client.Info.SubProfileInfo.SubProfileId).Cdkeyenc = _request.CDKeyEnc;

                db.SaveChanges();
            }
        }
    }
}
