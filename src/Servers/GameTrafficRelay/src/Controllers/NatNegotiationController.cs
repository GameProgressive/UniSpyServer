using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using UniSpy.Server.NatNegotiation.Aggregate.GameTrafficRelay;
using UniSpy.Server.NatNegotiation.Handler.CmdHandler;

namespace UniSpy.Server.GameTrafficRelay.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class NatNegotiationController : ControllerBase
    {
        private readonly ILogger<NatNegotiationController> _logger;
        public NatNegotiationController(ILogger<NatNegotiationController> logger)
        {
            _logger = logger;
        }
        [HttpPost]
        public Task<NatNegotiationResponse> GetNatNegotiationInfo(NatNegotiationRequest request)
        {
            NatNegotiationResponse response;
            if (request.GameClientIPs is null || request.GameServerIPs is null)
            {
                response = new NatNegotiationResponse()
                {
                    Port = -1,
                    Message = "game client/server's address is missing from request"
                };
                return Task.FromResult(response);
            }
            // natneg connecthandler will send 2 request to game traffic relay
            ConnectionListener listener;
            lock (PingHandler.ConnectionListeners)
            {
                if (!PingHandler.ConnectionListeners.TryGetValue(request.Cookie, out listener))
                {
                    var relayEnd = NetworkUtils.GetAvaliableLocalEndPoint();
                    listener = new ConnectionListener(relayEnd,
                                                    request.Cookie,
                                                    request.GameServerIPs,
                                                    request.GameClientIPs);

                    PingHandler.ConnectionListeners.TryAdd(request.Cookie, listener);
                }
            }
            response = new NatNegotiationResponse()
            {
                Port = listener.ListeningEndPoint.Port
            };
            return Task.FromResult(response);
        }
    }
}


