using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Structure.Request;
using System.Linq;

namespace GameStatus.Handler.CmdHandler
{
    /// <summary>
    /// Set persist storage data
    /// </summary>
    internal sealed class SetPDHandler : GSCmdHandlerBase
    {
        private new SetPDRequest _request => (SetPDRequest)base._request;
        public SetPDHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {

                var result = from p in db.Pstorage
                             where p.Profileid == _request.ProfileID
                             && p.Dindex == _request.DataIndex
                             && p.Ptype == (uint)_request.StorageType
                             select p;

                Pstorage ps;
                if (result.Count() == 0)
                {
                    //insert a new record in database
                    ps = new Pstorage();
                    ps.Dindex = _request.DataIndex;
                    ps.Profileid = _request.ProfileID;
                    ps.Ptype = (uint)_request.StorageType;
                    ps.Data = _request.KeyValueString;
                    db.Pstorage.Add(ps);
                }
                else if (result.Count() == 1)
                {
                    //update an existed record in database
                    ps = result.First();
                    ps.Data = _request.KeyValueString;
                }

                db.SaveChanges();
            }
        }

        protected override void ResponseConstruct()
        {
            throw new System.NotImplementedException();
        }
    }
}
