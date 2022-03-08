using System.Linq;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler
{
    [HandlerContract("addblock")]
    public sealed class AddBlockHandler : Abstraction.BaseClass.CmdHandlerBase
    {
        private new AddBlockRequest _request => (AddBlockRequest)base._request;
        public AddBlockHandler(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void DataOperation()
        {
            using (var db = new UniSpyContext())
            {
                if (db.Blockeds.Where(b => b.Targetid == _request.ProfileId
                && b.Namespaceid == _client.Info.SubProfileInfo.NamespaceId
                && b.ProfileId == _client.Info.ProfileInfo.ProfileId).Count() == 0)
                {
                    Blocked blocked = new Blocked
                    {
                        ProfileId = (int)_client.Info.ProfileInfo.ProfileId,
                        Targetid = _request.ProfileId,
                        Namespaceid = (int)_client.Info.SubProfileInfo.NamespaceId
                    };
                    db.Blockeds.Update(blocked);
                }
            }
        }
    }
}
