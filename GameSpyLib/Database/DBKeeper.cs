using System;
using System.Timers;

namespace GameSpyLib.Database
{
    public class DBKeeper
    {
        private Timer _dbKeepTimer = new Timer();
        private DatabaseDriver _db;

        public DBKeeper(DatabaseDriver db)
        {
            _db = db;
            _dbKeepTimer.Enabled = true;
            _dbKeepTimer.Interval = 3600000;
            //_dbKeepTimer.Interval = 1000;            
            _dbKeepTimer.AutoReset = true;
        }
        public void Run()
        {
            _dbKeepTimer.Start();
            _dbKeepTimer.Elapsed += KeepAliveQuery;
        }
        private void KeepAliveQuery(object sender, ElapsedEventArgs e)
        {
            _db.Query("SELECT COUNT(*) FROM users");
            Logging.LogWriter.Log.Write("[DB] keep alive sended!",Logging.LogLevel.Info);
        }
    }
}
