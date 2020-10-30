using GameSpyLib.Common;
using GameSpyLib.Abstraction.Interface;
using Newtonsoft.Json;
using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.NatNeg;
using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Entity.Structure.Packet.Request;
using StackExchange.Redis;
using System.Linq;
using GameSpyLib.Abstraction.BaseClass;

namespace ServerBrowser.Handler.CommandHandler.NatNeg
{
    /// <summary>
    /// we need forward this to game server
    /// </summary>
    public class NatNegCookieHandler : SBCommandHandlerBase
    {
        new SBSession _session;
        private AdHocRequest _request;
        private NatNegCookie _natNegCookie;
        private GameServer _gameServer;
        public NatNegCookieHandler(ISession session, byte[] recv) : base(session, recv)
        {
            _session = (SBSession)session;
            _natNegCookie = new NatNegCookie();
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();
            if (_session.ServerMessageList.Count == 0)
            {
                _errorCode = SBErrorCode.Parse;
                return;
            }
            _request = _session.ServerMessageList[0];
            _session.ServerMessageList.Remove(_request);

            var result = GameServer.GetServers(_request.TargetServerIP)
                .Where(s => s.ServerData.KeyValue.ContainsKey("hostport"))
                .Where(s => s.ServerData.KeyValue["hostport"] == _request.TargetServerHostPort);

            if (result.Count() != 1)
            {
                _errorCode = SBErrorCode.NoServersFound;
                return;
            }
            _gameServer = result.FirstOrDefault();
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            _natNegCookie.GameServerRemoteIP = _request.TargetServerIP;
            _natNegCookie.GameServerRemotePort = _gameServer.RemoteQueryReportPort;
            _natNegCookie.NatNegMessage = _recv;
        }

        protected override void ConstructResponse()
        {
            base.ConstructResponse();
        }

        protected override void Response()
        {
            ISubscriber sub = ServerManagerBase.Redis.GetSubscriber();

            string jsonStr = JsonConvert.SerializeObject(_natNegCookie);

            sub.Publish("NatNegCookieChannel", jsonStr);
        }
    }
}
