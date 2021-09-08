using NatNegotiation.Abstraction.BaseClass;
using NatNegotiation.Application;
using NatNegotiation.Entity.Exception;
using NatNegotiation.Entity.Structure.Redis;
using NatNegotiation.Entity.Structure.Request;
using NatNegotiation.Entity.Structure.Response;
using NatNegotiation.Entity.Structure.Result;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using NatNegotiation.Entity.Contract;
using NatNegotiation.Entity.Enumerate;

namespace NatNegotiation.Handler.CmdHandler
{
    [HandlerContract(RequestType.Connect)]
    internal sealed class ConnectHandler : CmdHandlerBase
    {
        private new ConnectRequest _request => (ConnectRequest)base._request;
        private new ConnectResult _result { get => (ConnectResult)base._result; set => base._result = value; }
        private Dictionary<NatUserInfoRedisKey, NatUserInfo> _negotiatorPairs;
        private List<NatUserInfoRedisKey> _matchedKeys;
        private ConnectResponse _responseToNegotiator;
        private ConnectResponse _responseToNegotiatee;
        private KeyValuePair<NatUserInfoRedisKey, NatUserInfo> _negotiator;
        private KeyValuePair<NatUserInfoRedisKey, NatUserInfo> _negotiatee;
        public ConnectHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new ConnectResult();
            _negotiatorPairs = new Dictionary<NatUserInfoRedisKey, NatUserInfo>();
        }

        protected override void RequestCheck()
        {
            var searchKey = new NatUserInfoRedisKey()
            {
                PortType = _request.PortType,
                Cookie = _request.Cookie
            };

            _matchedKeys = NatUserInfoRedisOperator.GetMatchedKeys(searchKey);
            // because cookie is unique for each client we will only get 2 of keys
            if (_matchedKeys.Count != 2)
            {
                throw new NNException("No users match found we continue waitting.");
            }


        }
        protected override void DataOperation()
        {
            foreach (var key in _matchedKeys)
            {
                _negotiatorPairs.Add(key, NatUserInfoRedisOperator.GetSpecificValue(key));

                ////find negitiators and negotiatees by a same cookie
                var negotiators = _negotiatorPairs.Where(s => s.Value.InitRequestInfo.ClientIndex == 0);
                var negotiatees = _negotiatorPairs.Where(s => s.Value.InitRequestInfo.ClientIndex == 1);

                if (negotiators.Count() != 1 || negotiatees.Count() != 1)
                {
                    LogWriter.ToLog("No match found, we keep waiting!");
                    return;
                }

                // we only can find one pair of the users
                _negotiator = negotiators.First();
                _negotiatee = negotiatees.First();

                LogWriter.Info($"Find negotiatee {_negotiatee.Value.RemoteEndPoint}");
                LogWriter.Info($"Find negotiator {_negotiator.Value.RemoteEndPoint}");

                var request = new ConnectRequest { Version = _request.Version, Cookie = _request.Cookie };
                _responseToNegotiator = new ConnectResponse(
                    request,
                    new ConnectResult { RemoteEndPoint = _negotiatee.Value.RemoteEndPoint });

                _responseToNegotiatee = new ConnectResponse(
                    _request,
                    new ConnectResult { RemoteEndPoint = _negotiator.Value.RemoteEndPoint });
            }
        }
        protected override void Response()
        {
            if (_responseToNegotiatee == null || _responseToNegotiator == null)
            {
                return;
            }
            _responseToNegotiatee.Build();
            _responseToNegotiator.Build();
            ServerFactory.Server.SendAsync(_negotiator.Value.RemoteEndPoint, _responseToNegotiator.SendingBuffer);
            ServerFactory.Server.SendAsync(_negotiatee.Value.RemoteEndPoint, _responseToNegotiatee.SendingBuffer);
        }

    }
}