using System.Linq;
using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.GameStatus.Handler.CmdHandler
{
    /// <summary>
    /// Save persist storage data
    /// No response for this handler
    /// </summary>
    public sealed class SetPlayerDataHandler : CmdHandlerBase
    {
        private new SetPlayerDataRequest _request => (SetPlayerDataRequest)base._request;
        public SetPlayerDataHandler(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void DataOperation()
        {
            using (var db = new UniSpyContext())
            {

                var result = from p in db.Pstorages
                             where p.ProfileId == _request.ProfileId
                             && p.Dindex == _request.DataIndex
                             && p.Ptype == (int)_request.StorageType
                             select p;

                Pstorage ps;
                if (result.Count() == 0)
                {
                    //insert a new record in database
                    ps = new Pstorage();
                    ps.Dindex = _request.DataIndex;
                    ps.ProfileId = _request.ProfileId;
                    ps.Ptype = (int)_request.StorageType;
                    ps.Data = _request.KeyValues;
                    db.Pstorages.Add(ps);
                }
                else if (result.Count() == 1)
                {
                    //update an existed record in database
                    ps = result.First();
                    ps.Data = _request.KeyValues;
                }

                db.SaveChanges();
            }
        }
    }
}
