using GameSpyLib.Logging;
using NatNegotiation;
using System;

namespace NATNegotiation.Handler.SystemHandler.ClientChecker
{
    public class ClientListChecker
    {
        private System.Timers.Timer _checkTimer = new System.Timers.Timer { Enabled = true, Interval = 10000, AutoReset = true };

        public void StartCheck(NatNegServer server)
        {
            _checkTimer.Start();
            _checkTimer.Elapsed += (s, e) => CheckClientTimeOut(server);
        }

        private void CheckClientTimeOut(NatNegServer server)
        {
            LogWriter.ToLog("Check timeout excuted!");

            foreach (var c in NatNegServer.ClientList.Values)
            {
                //Console.WriteLine(DateTime.Now.Subtract(c.LastPacketTime).TotalSeconds);
                if (DateTime.Now.Subtract(c.LastPacketTime).TotalSeconds > 60)
                {
                    LogWriter.ToLog("Deleted client " + c.RemoteEndPoint.ToString());
                    NatNegServer.ClientList.TryRemove(c.RemoteEndPoint, out _);
                }
            }
        }
    }
}
