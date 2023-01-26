using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UniSpyServer.Servers.GameTrafficRelay.Entity;
using UniSpyServer.Servers.GameTrafficRelay.Entity.Structure;

namespace UniSpyServer.Servers.GameTrafficRelay.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class NatNegotiationController : ControllerBase
    {
        private readonly ILogger<NatNegotiationController> _logger;
        public static IDictionary<uint, ConnectionPair> ConnectionPairs = new ConcurrentDictionary<uint, ConnectionPair>();
        public NatNegotiationController(ILogger<NatNegotiationController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public NatNegotiationResponse GetNatNegotiationInfo(NatNegotiationRequest request)
        {
            var address = this.HttpContext.Connection.LocalIpAddress;
            if (address is null)
            {
                throw new Exception("hosting address is null");
            }

            // natneg connecthandler will send 2 request to game traffic relay
            ConnectionPair pair;
            lock (ConnectionPairs)
            {
                if (!ConnectionPairs.TryGetValue(request.Cookie, out pair))
                {
                    var ports = NetworkUtils.GetAvailablePorts();
                    var ends = new IPEndPoint[]{new IPEndPoint(address,ports[0]),
                                        new IPEndPoint(address,ports[1])};
                    pair = new ConnectionPair(ends[0], ends[1], request.Cookie);
                    ConnectionPairs.TryAdd(request.Cookie, pair);
                }
            }

            var response = new NatNegotiationResponse()
            {
                IPEndPoint1 = pair.Listener1.ListeningEndPoint,
                IPEndPoint2 = pair.Listener2.ListeningEndPoint
            };
            return response;
        }
    }
}


