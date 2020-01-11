using GameSpyLib.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace StatsAndTracking.Handler.CommandHandler
{
    public class GStatsHandlerBase : HandlerBase<GStatsServer, Dictionary<string, string>>
    {
        protected Dictionary<string, string> _recv;
        protected ushort operationID;
        protected GStatsHandlerBase(Dictionary<string,string> recv)
        {
            _recv = recv;
        }

        public override void Handle(GStatsServer source)
        {
            throw new NotImplementedException();
        }

        protected virtual void CheckRequest()
        { }

    }
}
