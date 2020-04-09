using System;
namespace GameSpyLib.Common.BaseClass
{
    public class CheckerBase <T1, T2>
    {
        private System.Timers.Timer _timer;

        public CheckerBase(double interval )
        {
            _timer  = new System.Timers.Timer { Enabled = true, Interval = interval, AutoReset = true };//10000
        }
        public CheckerBase()
        {
            _timer = new System.Timers.Timer { Enabled = true, Interval = 10000, AutoReset = true };//10000
        }
        public void StartCheck(T1 source, T2 param)
        {
            _timer.Start();
            _timer.Elapsed += (s, e) => CheckClientTimeOut(source);
        }

        protected virtual void CheckClientTimeOut(T1 source)
        {
            
        }
    }
}
