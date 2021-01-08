using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using PresenceConnectionManager.Entity.Structure.Request;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Linq;
using PresenceConnectionManager.Abstraction.BaseClass;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    public class StatusInfoHandler : PCMCmdHandlerBase
    {
        protected new StatusInfoRequest _request => (StatusInfoRequest)base._request;

        public StatusInfoHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                var result = db.Statusinfo
                    .Where(s => s.Profileid == _session.UserInfo.ProfileID
                    && s.Namespaceid == _session.UserInfo.NamespaceID)
                    .Select(s => s);

                if (result.Count() == 0)
                {
                    Statusinfo statusinfo = new Statusinfo
                    {
                        Profileid = _session.UserInfo.ProfileID,
                        Namespaceid = _session.UserInfo.NamespaceID,
                        Productid = _session.UserInfo.ProductID,
                        Statusstate = _request.StatusState,
                        //buddyip
                        //buddyport
                        Hostip = _request.HostIP,
                        Hostprivateip = _request.HostPrivateIP,
                        Queryreport = _request.QueryReportPort,
                        Hostport = _request.HostPort,
                        Sessionflags = _request.SessionFlags,
                        Richstatus = _request.RichStatus,
                        Gametype = _request.GameType,
                        Gamevariant = _request.GameVariant,
                        Gamemapname = _request.GameMapName,
                        Quietmodefalgs = _request.QuietModeFlags
                    };

                    db.Statusinfo.Add(statusinfo);

                    db.SaveChanges();
                }
                else if (result.Count() == 1)
                {

                    result.First().Profileid = _session.UserInfo.ProfileID;
                    result.First().Namespaceid = _session.UserInfo.NamespaceID;
                    result.First().Productid = _session.UserInfo.ProductID;
                    result.First().Statusstate = _request.StatusState;
                    //buddyip
                    //buddyport
                    result.First().Hostip = _request.HostIP;
                    result.First().Hostprivateip = _request.HostPrivateIP;
                    result.First().Queryreport = _request.QueryReportPort;
                    result.First().Hostport = _request.HostPort;
                    result.First().Sessionflags = _request.SessionFlags;
                    result.First().Richstatus = _request.RichStatus;
                    result.First().Gametype = _request.GameType;
                    result.First().Gamevariant = _request.GameVariant;
                    result.First().Gamemapname = _request.GameMapName;
                    result.First().Quietmodefalgs = _request.QuietModeFlags;

                    db.SaveChanges();
                }
                else
                {
                    _result.ErrorCode = GPErrorCode.DatabaseError;
                }
            }
        }
    }
}
