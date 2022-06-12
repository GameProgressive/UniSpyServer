using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
            _result.RemoteIPEndPoint = _client.Session.RemoteIPEndPoint;
        }
        protected override void ResponseConstruct()
        {
            _response = new InitResponse(_request, _result);
        }
        public static bool IsInSameLan(Dictionary<NatPortType, NatInitInfo> clientPackets, Dictionary<NatPortType, NatInitInfo> serverPackets)
        {

            // todo private address should compare only xxx.xxx.xxx no need for last byte compare
            return clientPackets[NatPortType.GP].PublicIPEndPoint.Address.Equals(serverPackets[NatPortType.GP].PublicIPEndPoint.Address)
            && clientPackets[NatPortType.GP].PrivateIPEndPoint.Address.GetAddressBytes().Take(3).ToArray().SequenceEqual(
                serverPackets[NatPortType.GP].PrivateIPEndPoint.Address.GetAddressBytes().Take(3).ToArray())

            && clientPackets[NatPortType.NN1].PublicIPEndPoint.Address.Equals(serverPackets[NatPortType.NN1].PublicIPEndPoint.Address)
            && clientPackets[NatPortType.NN1].PrivateIPEndPoint.Address.GetAddressBytes().Take(3).ToArray().SequenceEqual(
                serverPackets[NatPortType.NN1].PrivateIPEndPoint.Address.GetAddressBytes().Take(3).ToArray())

            && clientPackets[NatPortType.NN2].PublicIPEndPoint.Address.Equals(serverPackets[NatPortType.NN2].PublicIPEndPoint.Address)
            && clientPackets[NatPortType.NN2].PrivateIPEndPoint.Address.GetAddressBytes().Take(3).ToArray().SequenceEqual(
                serverPackets[NatPortType.NN2].PrivateIPEndPoint.Address.GetAddressBytes().Take(3).ToArray())

            && clientPackets[NatPortType.NN3].PublicIPEndPoint.Address.Equals(serverPackets[NatPortType.NN3].PublicIPEndPoint.Address)
            && clientPackets[NatPortType.NN3].PrivateIPEndPoint.Address.GetAddressBytes().Take(3).ToArray().SequenceEqual(
                serverPackets[NatPortType.NN3].PrivateIPEndPoint.Address.GetAddressBytes().Take(3).ToArray());

        }
        public static NatProperty DetermineNatType(Dictionary<NatPortType, NatInitInfo> initResults)
        {
            NatProperty natProperty = new NatProperty();
            if (initResults.Count != 4)
            {
                throw new NNException("We need 4 init results to determine the nat type.");
            }
            var gp = initResults[NatPortType.GP];
            var nn1 = initResults[NatPortType.NN1];
            var nn2 = initResults[NatPortType.NN2];
            var nn3 = initResults[NatPortType.NN3];

            // no nat
            if (gp.PublicIPEndPoint.Address.Equals(gp.PrivateIPEndPoint.Address)
            && nn1.PublicIPEndPoint.Address.Equals(nn1.PrivateIPEndPoint.Address)
            && nn2.PublicIPEndPoint.Equals(nn2.PrivateIPEndPoint)
            && nn3.PublicIPEndPoint.Equals(nn3.PrivateIPEndPoint))
            {
                natProperty.NatType = NatType.NoNat;
                return natProperty;
            }
            // detect cone
            else if (gp.PublicIPEndPoint.Equals(nn1.PublicIPEndPoint)
            && nn1.PublicIPEndPoint.Equals(nn2.PublicIPEndPoint)
            && nn2.PublicIPEndPoint.Equals(nn3.PublicIPEndPoint))
            {
                natProperty.NatType = NatType.FullCone;
                return natProperty;
            }
            else if (!gp.PublicIPEndPoint.Equals(nn1.PublicIPEndPoint)
            || !nn1.PublicIPEndPoint.Equals(nn2.PublicIPEndPoint)
            || !nn2.PublicIPEndPoint.Equals(nn3.PublicIPEndPoint))
            {
                natProperty.NatType = NatType.Symmetric;
                natProperty.PortMapping = NatPortMappingScheme.Incremental;
                natProperty.PortIncrement = Math.Abs(gp.PublicIPEndPoint.Port - nn1.PublicIPEndPoint.Port);
                return natProperty;
            }
            else
            {
                natProperty.NatType = NatType.Unknown;
                throw new NNException("Unknow nat type.");
            }
        }

        public static IPEndPoint GuessTargetAddress(NatProperty property, Dictionary<NatPortType, NatInitInfo> initResults, IPEndPoint natFailedEd = null)
        {
            Debug.WriteLine(property.NatType.ToString() + property.PortMapping.ToString());
            switch (property.NatType)
            {
                case NatType.NoNat:
                case NatType.FirewallOnly:
                    // private is public
                    return initResults[NatPortType.NN1].PublicIPEndPoint;
                case NatType.FullCone:
                    return new IPEndPoint(initResults[NatPortType.NN1].PublicIPEndPoint.Address, initResults[NatPortType.NN1].PublicIPEndPoint.Port);
                case NatType.Symmetric:
                    switch (property.PortMapping)
                    {
                        case NatPortMappingScheme.Incremental:
                            return new IPEndPoint(initResults[NatPortType.NN3].PublicIPEndPoint.Address,
                                                    initResults[NatPortType.NN3].PublicIPEndPoint.Port + property.PortIncrement);
                        default:
                            return null;
                    }
                default:
                    return null;
            }
        }
    }
}
