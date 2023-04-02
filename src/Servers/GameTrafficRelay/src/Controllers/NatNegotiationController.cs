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
        public static ConcurrentDictionary<uint, ConnectionPair> ConnectionPairs = new ConcurrentDictionary<uint, ConnectionPair>();
        public NatNegotiationController(ILogger<NatNegotiationController> logger)
        {
            _logger = logger;
        }
        [HttpPost]
        public Task<NatNegotiationResponse> GetNatNegotiationInfo(NatNegotiationRequest request)
        {
            // natneg connecthandler will send 2 request to game traffic relay
            ConnectionPair pair;

            if (!ConnectionPairs.TryGetValue(request.Cookie, out pair))
            {
                var ports = NetworkUtils.GetAvailablePorts();
                var ends = new IPEndPoint[]{new IPEndPoint(IPAddress.Any,ports[0]),
                                                new IPEndPoint(IPAddress.Any,ports[1])};
                pair = new ConnectionPair(ends[0], ends[1], request.Cookie);
                ConnectionPairs.TryAdd(request.Cookie, pair);
            }

            var response = new NatNegotiationResponse()
            {
                IPEndPoint1 = new IPEndPoint(ServerLauncher.Server.PublicIPEndPoint.Address, pair.Listener1.ListeningEndPoint.Port),
                IPEndPoint2 = new IPEndPoint(ServerLauncher.Server.PublicIPEndPoint.Address, pair.Listener2.ListeningEndPoint.Port)
            };
            return Task.FromResult(response);
        }
    }
}


