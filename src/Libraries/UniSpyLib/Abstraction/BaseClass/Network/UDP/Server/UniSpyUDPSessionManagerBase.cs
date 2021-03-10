using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using UniSpyLib.Logging;
using UniSpyLib.Network;

namespace UniSpyLib.Abstraction.BaseClass.Network.UDP
{
    public abstract class UniSpyUDPSessionManagerBase : UniSpySessionManagerBase
    {
        protected TimeSpan _expireTimeInterval { get; set; }
        private Timer _timer;
        public UniSpyUDPSessionManagerBase()
        {
            //default settings
            _timer = new Timer
            {
                Enabled = true,
                Interval = 60000,
                AutoReset = true
            };//10000
            _expireTimeInterval = new TimeSpan(0, 0, 120);
            _timer.Start();
            _timer.Elapsed += (s, e) => CheckExpiredSession();
        }

        protected virtual void CheckExpiredSession()
        {
            //log which expire manager excuted
            LogWriter.LogCurrentClass(this);
            Parallel.ForEach(SessionPool.Values, (session) =>
            {
                UniSpyUDPSessionBase sess = (UniSpyUDPSessionBase)session;
                // we calculate the interval between last packe and current time
                if (sess.SessionExistedTime > _expireTimeInterval)
                {
                    SessionPool.TryRemove(sess.RemoteIPEndPoint, out _);
                }
            });
        }
    }
}
