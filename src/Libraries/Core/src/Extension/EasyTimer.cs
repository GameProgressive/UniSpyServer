using System;
using System.Timers;

namespace UniSpy.Server.Core.Extension
{
    public class EasyTimer
    {
        public TimeSpan? ExpireTime { get; private set; }
        public DateTime LastActiveTime { get; private set; }
        public TimeSpan IdleTime => DateTime.Now - LastActiveTime;
        /// <summary>
        /// The timer to count and invoke some event
        /// </summary>
        private Timer _timer;
        public bool IsExpired
        {
            get
            {
                if (ExpireTime is null)
                {
                    return false;
                }
                else
                {
                    return IdleTime > ExpireTime;
                }
            }
        }
        public event ElapsedEventHandler Elapsed
        {
            add { _timer.Elapsed += value; }
            remove { _timer.Elapsed -= value; }
        }
        /// <summary>
        /// Easy timer constructor, remember to call Start()
        /// </summary>
        public EasyTimer(TimeSpan? expireTimeSpan, TimeSpan intervalTimeSpan)
        {
            _timer = new Timer
            {
                Enabled = true,
                Interval = intervalTimeSpan.TotalMilliseconds,
                AutoReset = true
            };
            ExpireTime = expireTimeSpan;
        }
        public EasyTimer(TimeSpan intervalTimeSpan) : this(null, intervalTimeSpan) { }
        /// <summary>
        /// Start the timer
        /// </summary>
        public void Start()
        {
            RefreshLastActiveTime();
            _timer.Start();
        }
        public void RefreshLastActiveTime() => LastActiveTime = DateTime.Now;
        public void Dispose() => _timer.Dispose();
    }
}