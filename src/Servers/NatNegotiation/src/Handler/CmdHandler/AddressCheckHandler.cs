using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
using UniSpy.Server.NatNegotiation.Enumerate;
using UniSpy.Server.NatNegotiation.Aggregate.Misc;
using UniSpy.Server.NatNegotiation.Aggregate.Redis;
using UniSpy.Server.NatNegotiation.Contract.Request;
using UniSpy.Server.NatNegotiation.Contract.Response;
using UniSpy.Server.NatNegotiation.Contract.Result;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.NatNegotiation.Handler.CmdHandler
{
    public sealed class AddressCheckHandler : CmdHandlerBase
    {
        private new AddressCheckRequest _request => (AddressCheckRequest)base._request;
        private new AddressCheckResult _result { get => (AddressCheckResult)base._result; set => base._result = value; }
        public AddressCheckHandler(IClient client, IRequest request) : base(client, request)
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
        public static NatProperty DetermineNatType(NatInitInfo iniInfo)
        {
            NatProperty natProperty = new NatProperty();
            if (iniInfo.AddressInfos.Count < 3)
            {
                throw new NatNegotiation.Exception("We need 3 init records to determine the nat type.");
            }
            var nn1 = iniInfo.AddressInfos[NatPortType.NN1];
            var nn2 = iniInfo.AddressInfos[NatPortType.NN2];
            var nn3 = iniInfo.AddressInfos[NatPortType.NN3];

            // no nat
            if (nn1.PublicIPEndPoint.Address.Equals(nn1.PrivateIPEndPoint.Address)
            && nn2.PublicIPEndPoint.Equals(nn2.PrivateIPEndPoint)
            && nn3.PublicIPEndPoint.Equals(nn3.PrivateIPEndPoint))
            {
                natProperty.NatType = NatType.NoNat;
                return natProperty;
            }
            // detect cone
            else if (nn1.PublicIPEndPoint.Equals(nn2.PublicIPEndPoint)
            && nn2.PublicIPEndPoint.Equals(nn3.PublicIPEndPoint))
            {
                natProperty.NatType = NatType.FullCone;
                return natProperty;
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
                return natProperty;
            }
            else
            {
                natProperty.NatType = NatType.Unknown;
                throw new NatNegotiation.Exception("Unknow nat type.");
            }
        }
    }
}
