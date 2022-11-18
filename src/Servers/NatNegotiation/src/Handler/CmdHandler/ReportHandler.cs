using System;
using System.Collections.Generic;
using System.Linq;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Exception;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Misc;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Redis;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Response;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.NatNegotiation.Handler.CmdHandler
{
    /// <summary>
    /// Get nat neg result report success or fail
    /// </summary>

    public sealed class ReportHandler : CmdHandlerBase
    {
        private new ReportRequest _request => (ReportRequest)base._request;
        private new ReportResult _result { get => (ReportResult)base._result; set => base._result = value; }
        /// <summary>
        /// The first layer key is the current client's public ip address, the second layer key is the current clients's private ip address, the third layer key is the other client's natneg server id, and the fourth key is the other client's private ip address
        /// the hash value of the same IPAddress is the same, so it can be used as the dictionary key.
        /// </summary>
        internal static Dictionary<string, NatPunchStrategy> NatFailRecordInfos;
        private NatInitInfo _myInitInfo;
        private NatInitInfo _othersInitInfo;
        private NatReportInfo _myReportInfo;
        private string _searchKey;
        static ReportHandler()
        {
            NatFailRecordInfos = new Dictionary<string, NatPunchStrategy>();
        }
        public ReportHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new ReportResult();
        }
        protected override void RequestCheck()
        {
            base.RequestCheck();
            var initInfos = _redisClient.Context.Where(k =>
                k.ServerID == _client.Connection.Server.ServerID
                && k.Cookie == _client.Info.Cookie).ToList();
            if (initInfos.Count != 2)
            {
                throw new NNException($"The number of init info in redis with cookie: {_client.Info.Cookie} is not equal to two.");
            }

            NatClientIndex otherClientIndex = (NatClientIndex)(1 - _client.Info.ClientIndex);
            _myInitInfo = initInfos.Where(i => i.ClientIndex == NatClientIndex.GameClient).First();
            _othersInitInfo = initInfos.Where(i => i.ClientIndex == otherClientIndex).First();
            _searchKey = NatReportInfo.CreateKey(_myInitInfo.PrivateIPEndPoint.Address, _myInitInfo.PublicIPEndPoint.Address, (Guid)_othersInitInfo.ServerID, _othersInitInfo.AddressInfos[NatPortType.NN3].PrivateIPEndPoint.Address);
        }
        protected override void ResponseConstruct()
        {
            _response = new ReportResponse(_request, _result);
        }

        protected override void Response()
        {
            // we first response, the client will timeout if no response is received in some interval
            base.Response();
            _client.LogInfo($"natneg success: {(bool)_request.IsNatSuccess}, version: {_request.Version}, gamename: {_request.GameName}, nat type: {_request.NatType} mapping scheme: {_request.MappingScheme}, cookie: {_request.Cookie}, client index: {_request.ClientIndex} port type: {_request.PortType}");
            // when negotiation failed we store the information
            if (!(bool)_request.IsNatSuccess)
            {
                lock (NatFailRecordInfos)
                {
                    if (NatFailRecordInfos.ContainsKey(_searchKey))
                    {
                        var strategy = NatFailRecordInfos[_searchKey];
                        // the final solusion is game relay service
                        if (strategy < NatPunchStrategy.UsingGameRelay)
                        {
                            strategy++;
                        }
                        NatFailRecordInfos[_searchKey] = strategy;
                        _client.LogInfo($"Updated negotiation strategy to {strategy}");
                    }
                    else
                    {
                        throw new NNException($"Client's info in NatFailRecordInfos did not created by ConnectHandler, please check search key: {_searchKey}");
                    }
                }

            }

            // we delete the information on redis
            // _redisClient.DeleteKeyValue(_myInitInfo);
            // _redisClient.DeleteKeyValue(_othersInitInfo);
        }
    }
}
