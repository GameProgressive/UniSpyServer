using System.Collections.Concurrent;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.NatNegotiation.Aggregate.GameTrafficRelay;
using UniSpy.Server.NatNegotiation.Contract.Request;

namespace UniSpy.Server.NatNegotiation.Handler.CmdHandler
{
    public class PingHandler : CmdHandlerBase
    {
        public static ConcurrentDictionary<uint, ConnectionListener> ConnectionListeners = new ConcurrentDictionary<uint, ConnectionListener>();
        public PingHandler(IClient client, PingRequest request) : base(client, request)
        {
        }
        protected override void RequestCheck()
        {
            base.RequestCheck();
        }
    }
}