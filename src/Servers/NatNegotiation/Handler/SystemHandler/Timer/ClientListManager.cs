using GameSpyLib.Abstraction.BaseClass;
using GameSpyLib.Logging;
using NatNegotiation.Server;
using System;

namespace NatNegotiation.Handler.SystemHandler.Timer
{
    public class ClientListManager : ExpireManagerBase
    {
        protected override void CheckExpire()
        {
            base.CheckExpire();

            foreach (var c in NatNegServer.Sessions.Values)
            {
                //Console.WriteLine(DateTime.Now.Subtract(c.LastPacketTime).TotalSeconds);
                if (DateTime.Now.Subtract(c.UserInfo.LastPacketRecieveTime).TotalSeconds > 60)
                {
                    LogWriter.ToLog("Deleted client " + c.RemoteEndPoint.ToString());
                    NatNegServer.Sessions.TryRemove(c.RemoteEndPoint, out _);
                }
            }
        }
    }
}
