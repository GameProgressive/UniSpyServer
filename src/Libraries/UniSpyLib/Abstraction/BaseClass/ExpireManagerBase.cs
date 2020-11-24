using UniSpyLib.Logging;
using System.Timers;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class TImerBase
    {
        protected static Timer _timer;
        /// <summary>
        /// Because every resource we manage is global
        /// so we do not parse any object to this class
        /// </summary>
        public TImerBase()
        {
            //default settings
            _timer = new Timer
            {
                Enabled = true,
                Interval = 60000,
                AutoReset = true
            };//10000
        }

        public virtual void Start()
        {
            _timer.Start();
            _timer.Elapsed += (s, e) => CheckExpire();
        }

        protected virtual void CheckExpire()
        {
            //log which expire manager excuted
            LogWriter.LogCurrentClass(this);

        }
    }
}
