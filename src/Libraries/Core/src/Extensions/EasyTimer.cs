using System;
using System.Timers;

namespace UniSpy.Server.Core.Extensions
{
    public class EasyTimer
    {
        public TimeSpan ExpireTime { get; private set; }
        public DateTime LastActiveTime { get; private set; }
        public TimeSpan IdleTime => DateTime.Now - LastActiveTime;
        /// <summary>
        /// The timer to count and invoke some event
        /// </summary>
        private Timer _timer;
        public bool IsExpired => IdleTime > ExpireTime;
        public EasyTimer(TimeSpan expireTimeSpan, TimeSpan intervalTimeSpan, Action invokingAction)
        {
            _timer = new Timer
            {
                Enabled = true,
                Interval = intervalTimeSpan.TotalMilliseconds,
                AutoReset = true
            };
            ExpireTime = expireTimeSpan;
            RefreshLastActiveTime();
            _timer.Elapsed += (s, e) => invokingAction();
            _timer.Start();
        }
        public void RefreshLastActiveTime() => LastActiveTime = DateTime.Now;
        public void Dispose() => _timer.Dispose();
    }
}