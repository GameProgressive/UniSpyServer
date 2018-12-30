using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameSpyLib.Database;
using GameSpyLib.Network;
using GameSpyLib.Server;

namespace RetroSpyServer.Server
{
    public class GPCMServer : PresenceServer
    {
        public GPCMServer(DatabaseDriver databaseDriver) : base(databaseDriver)
        {
        }

        protected override void OnException(Exception e)
        {
            throw new NotImplementedException();
        }

        protected override void ProcessAccept(TCPStream Stream)
        {
            throw new NotImplementedException();
        }
    }
}
