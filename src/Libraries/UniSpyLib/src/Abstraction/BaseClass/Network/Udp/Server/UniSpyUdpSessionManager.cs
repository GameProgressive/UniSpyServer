using System;
using System.Threading.Tasks;
using System.Timers;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Udp.Server
{
    public abstract class UniSpyUdpSessionManager : UniSpySessionManager
    {
        protected TimeSpan _expireTimeInterval { get; set; }
        private Timer _timer;
        public UniSpyUdpSessionManager()
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
                UniSpyUdpSession sess = (UniSpyUdpSession)session;
                // we calculate the interval between last packe and current time
                if (sess.SessionExistedTime > _expireTimeInterval)
                {
                    SessionPool.TryRemove(sess.RemoteIPEndPoint, out _);
                }
            });
        }
    }
}
