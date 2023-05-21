using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
using UniSpy.Server.NatNegotiation.Enumerate;
using UniSpy.Server.NatNegotiation.Aggregate.Misc;
using UniSpy.Server.NatNegotiation.Aggregate.Redis;
using UniSpy.Server.NatNegotiation.Contract.Request;
using UniSpy.Server.NatNegotiation.Contract.Response;
using UniSpy.Server.NatNegotiation.Contract.Result;
using UniSpy.Server.NatNegotiation.Application;

namespace UniSpy.Server.NatNegotiation.Handler.CmdHandler
{
    public sealed class AddressCheckHandler : CmdHandlerBase
    {
        private new AddressCheckRequest _request => (AddressCheckRequest)base._request;
        private new AddressCheckResult _result { get => (AddressCheckResult)base._result; set => base._result = value; }
        public AddressCheckHandler(Client client, AddressCheckRequest request) : base(client, request)
        {
            _result = new AddressCheckResult();
        }
        protected override void DataOperation()
        {
            _result.RemoteIPEndPoint = _client.Connection.RemoteIPEndPoint;
        }
        protected override void ResponseConstruct()
        {
            _response = new InitResponse(_request, _result);
        }
        public static NatProperty DetermineNatTypeVersion2(NatInitInfo initInfo)
        {
            NatProperty natProperty = new NatProperty();
            if (initInfo.AddressInfos.Count < 3)
            {
                throw new NatNegotiation.Exception("We need 3 init records to determine the nat type.");
            }
            var nn1 = initInfo.AddressInfos[NatPortType.NN1];
            var nn2 = initInfo.AddressInfos[NatPortType.NN2];

            // no nat
            if (nn1.PublicIPEndPoint.Address.Equals(nn1.PrivateIPEndPoint.Address)
            && nn2.PublicIPEndPoint.Equals(nn2.PrivateIPEndPoint))
            {
                natProperty.NatType = NatType.NoNat;
            }
            // detect cone
            else if (nn1.PublicIPEndPoint.Equals(nn2.PublicIPEndPoint))
            {
                natProperty.NatType = NatType.FullCone;
            }
            else if (!nn1.PublicIPEndPoint.Equals(nn2.PublicIPEndPoint))
            {
                natProperty.NatType = NatType.Symmetric;
                natProperty.PortMapping = NatPortMappingScheme.Unrecognized;
                //todo get all interval of the port increment value
                var portIncrement1 = nn2.PublicIPEndPoint.Port - nn1.PublicIPEndPoint.Port;
            }
            else
            {
                natProperty.NatType = NatType.Unknown;
            }
            return natProperty;
        }
        public static NatProperty DetermineNatTypeVersion3(NatInitInfo initInfo)
        {
            NatProperty natProperty = new NatProperty();
            if (initInfo.AddressInfos.Count < 3)
            {
                throw new NatNegotiation.Exception("We need 3 init records to determine the nat type.");
            }
            var nn1 = initInfo.AddressInfos[NatPortType.NN1];
            var nn2 = initInfo.AddressInfos[NatPortType.NN2];
            var nn3 = initInfo.AddressInfos[NatPortType.NN3];

            // no nat
            if (nn1.PublicIPEndPoint.Address.Equals(nn1.PrivateIPEndPoint.Address)
            && nn2.PublicIPEndPoint.Equals(nn2.PrivateIPEndPoint)
            && nn3.PublicIPEndPoint.Equals(nn3.PrivateIPEndPoint))
            {
                natProperty.NatType = NatType.NoNat;
            }
            // detect cone
            else if (nn1.PublicIPEndPoint.Equals(nn2.PublicIPEndPoint)
            && nn2.PublicIPEndPoint.Equals(nn3.PublicIPEndPoint))
            {
                natProperty.NatType = NatType.FullCone;
            }
            else if (!nn1.PublicIPEndPoint.Equals(nn2.PublicIPEndPoint)
            || !nn2.PublicIPEndPoint.Equals(nn3.PublicIPEndPoint))
            {
                natProperty.NatType = NatType.Symmetric;
                natProperty.PortMapping = NatPortMappingScheme.Incremental;
                //todo get all interval of the port increment value
                var portIncrement1 = nn2.PublicIPEndPoint.Port - nn1.PublicIPEndPoint.Port;
                var portIncrement2 = nn3.PublicIPEndPoint.Port - nn2.PublicIPEndPoint.Port;
                if (portIncrement1 == portIncrement2)
                {
                    natProperty.PortIncrement = portIncrement1;
                }
                else
                {
                    var increaseInterval = portIncrement2 - portIncrement1;
                    natProperty.PortIncrement = portIncrement2 + increaseInterval;
                }
            }
            else
            {
                natProperty.NatType = NatType.Unknown;

            }
            return natProperty;
        }
    }
}
