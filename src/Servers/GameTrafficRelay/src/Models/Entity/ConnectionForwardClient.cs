using System.Linq;
using System.Net;
using System.Threading;
using UniSpyServer.Servers.GameTrafficRelay.Interface;
using UniSpyServer.UniSpyLib.Extensions;
using UniSpyServer.UniSpyLib.Logging;
using System;
namespace UniSpyServer.Servers.GameTrafficRelay.Entity.Structure
{
    public sealed class ConnectionForwardClient : IConnectionForwardClient
    {
        public IConnectionListener GameSpyTrafficListener { get; set; }
        public IConnectionForwardClient ForwardTargetClient { get; set; }
        public ConnectionForwardClient()
        {
        }
        public void ForwardMessage(byte[] data)
        {
            LogWriter.Debug($" [{GameSpyTrafficListener.ListeningEndPoint}] => [{ForwardTargetClient.GameSpyTrafficListener.ListeningEndPoint}] {StringExtensions.ConvertPrintableCharToString(data)} [{StringExtensions.ConvertByteToHexString(data)}]");
            ForwardTargetClient.ForwardMessage(data);
        }
    }
}