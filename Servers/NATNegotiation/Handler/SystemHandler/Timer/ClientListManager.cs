using GameSpyLib.Common.BaseClass;
using GameSpyLib.Logging;
using NatNegotiation;
using NatNegotiation.Server;
using System;

namespace NatNegotiation.Handler.SystemHandler.Timer
{
    public class ClientListManager : ExpireManagerBase
    {
        protected override void CheckExpire()
        {
            base.CheckExpire();

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
