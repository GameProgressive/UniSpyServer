using System;
using System.Collections.Generic;
using QueryReport.Entity.Structure;

namespace ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionHandler.NoServerList
{
    /// <summary>
    /// No server list update option only get ip and host port for client
    /// so we do not need to implement server key, info, uniquevalue stuff
    /// </summary>
    public class NoServerListHandler : UpdateOptionHandlerBase
    {
        public NoServerListHandler(SBSession session, byte[] recv) : base(session, recv)
        {
        }
    }
}
