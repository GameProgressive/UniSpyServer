using System.Linq;
using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
using UniSpy.Server.NatNegotiation.Application;
using UniSpy.Server.NatNegotiation.Enumerate;
using UniSpy.Server.NatNegotiation.Aggregate.Redis;
using UniSpy.Server.NatNegotiation.Contract.Request;
using UniSpy.Server.NatNegotiation.Contract.Response;
using UniSpy.Server.NatNegotiation.Contract.Result;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.NatNegotiation.Handler.CmdHandler
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
        // internal static Dictionary<string, NatPunchStrategy> NatFailRecordInfos;
        private NatInitInfo _myInitInfo;
        private NatInitInfo _othersInitInfo;
        // private string _searchKey;
        static ReportHandler()
        {
            // NatFailRecordInfos = new Dictionary<string, NatPunchStrategy>();
        }
        public ReportHandler(Client client, ReportRequest request) : base(client, request)
        {
            _result = new ReportResult();
        }
        protected override void RequestCheck()
        {
            base.RequestCheck();
            var addressInfos = StorageOperation.Persistance.GetInitInfos(_client.Server.Id, (uint)_request.Cookie);
            if (addressInfos.Count < InitHandler.InitPacketCount)
            {
                throw new NatNegotiation.Exception($"The number of init info in redis with cookie: {_request.Cookie} is not bigger than 6.");
            }

            NatClientIndex otherClientIndex = (NatClientIndex)(1 - _request.ClientIndex);
            // we need both info to determine nat type
            _othersInitInfo = new NatInitInfo(addressInfos.Where(i => i.ClientIndex == otherClientIndex).OrderBy(i => i.PortType).ToList());
            _myInitInfo = new NatInitInfo(addressInfos.Where(i => i.ClientIndex == _request.ClientIndex).OrderBy(i => i.PortType).ToList());
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
                //todo we save the fail information some where else.
                var info = new NatNegotiation.Aggregate.Redis.Fail.NatFailInfo(_myInitInfo, _othersInitInfo);
                if (StorageOperation.Persistance.GetNatFailInfo(info) == 0)
                {
                    StorageOperation.Persistance.UpdateNatFailInfo(info);
                }
            }
        }
    }
}
