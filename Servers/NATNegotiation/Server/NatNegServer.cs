using GameSpyLib.Logging;
using GameSpyLib.Network;
using NatNegotiation.Handler.CommandHandler.CommandSwitcher;
using NatNegotiation.Entity.Structure;
using NatNegotiation.Handler.SystemHandler.Timer;
using System;
using System.Collections.Concurrent;
using System.Net;

namespace NatNegotiation
{
    public class NatNegServer : TemplateUdpServer
    {
        public static ConcurrentDictionary<EndPoint, ClientInfo> ClientList = new ConcurrentDictionary<EndPoint, ClientInfo>();

        public NatNegServer(IPAddress address, int port) : base(address, port)
        { 
        }

        public override bool Start()
        {
            new ClientListManager().Start();
            return base.Start();
        }

        protected override void OnReceived(EndPoint endPoint, byte[] message)
        {
            CommandSwitcher.Switch(this,endPoint,message);
        }

        private void CheckClientTimeOut(object sender, System.Timers.ElapsedEventArgs e)
        {
            LogWriter.ToLog("Check timeout excuted!");

            foreach (var c in ClientList.Values)
            {
                //Console.WriteLine(DateTime.Now.Subtract(c.LastPacketTime).TotalSeconds);
                if (DateTime.Now.Subtract(c.LastPacketTime).TotalSeconds > 60)
                {
                    LogWriter.ToLog("Deleted client " + c.RemoteEndPoint.ToString());
                    ClientList.TryRemove(c.RemoteEndPoint, out _);
                }
            }
        }
    }
}
