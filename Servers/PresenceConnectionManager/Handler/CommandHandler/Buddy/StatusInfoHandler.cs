using System.Collections.Generic;
using System.Linq;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceConnectionManager.Entity.Structure.Request.Buddy;
using PresenceSearchPlayer.Entity.Enumerator;

namespace PresenceConnectionManager.Handler.CommandHandler.Buddy
{
    public class StatusInfoHandler : PCMCommandHandlerBase
    {
        protected StatusInfoRequest _request;
        public StatusInfoHandler(ISession session, Dictionary<string, string> recv) : base(session, recv)
        {
            _request = new StatusInfoRequest(recv);
        }

        protected override void CheckRequest()
        {
            _errorCode = _request.Parse();
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            using (var db = new retrospyContext())
            {
                var result = db.Statusinfo
                    .Where(s => s.Profileid == _session.UserData.ProfileID
                    && s.Namespaceid == _session.UserData.NamespaceID)
                    .Select(s => s);



                if (result.Count() == 0)
                {
                    Statusinfo statusinfo = new Statusinfo
                    {
                        Profileid = _session.UserData.ProfileID,
                        Namespaceid = _session.UserData.NamespaceID,
                        Productid = _session.UserData.ProductID,
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

                    result.First().Profileid = _session.UserData.ProfileID;
                    result.First().Namespaceid = _session.UserData.NamespaceID;
                    result.First().Productid = _session.UserData.ProductID;
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
                    _errorCode = GPError.DatabaseError;
                }

            }
        }
    }
}
