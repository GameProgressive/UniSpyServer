using System.Collections.Concurrent;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UniSpy.Server.GameTrafficRelay.Application;
using UniSpy.Server.GameTrafficRelay.Entity;
using UniSpy.Server.GameTrafficRelay.Aggregate;
using System.Threading.Tasks;

namespace UniSpy.Server.GameTrafficRelay.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class NatNegotiationController : ControllerBase
    {
        private readonly ILogger<NatNegotiationController> _logger;
        public static ConcurrentDictionary<uint, ConnectionListener> ConnectionListeners = new ConcurrentDictionary<uint, ConnectionListener>();
        public NatNegotiationController(ILogger<NatNegotiationController> logger)
        {
            _logger = logger;
        }
        [HttpPost]
        public Task<NatNegotiationResponse> GetNatNegotiationInfo(NatNegotiationRequest request)
        {
            NatNegotiationResponse response;
            if (request.GameClientIP is null || request.GameServerIP is null)
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
            lock (ConnectionListeners)
            {
                if (!ConnectionListeners.TryGetValue(request.Cookie, out listener))
                {
                    var relayEnd = NetworkUtils.GetAvaliableLocalEndPoint();
                    listener = new ConnectionListener(
                    relayEnd,
                    request.Cookie,
                    IPEndPoint.Parse(request.GameServerIP).Address,
                    IPEndPoint.Parse(request.GameClientIP).Address);

                    ConnectionListeners.TryAdd(request.Cookie, listener);
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


