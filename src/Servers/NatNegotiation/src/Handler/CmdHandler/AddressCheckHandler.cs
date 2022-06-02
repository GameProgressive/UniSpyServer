using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Contract;
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
    [HandlerContract(RequestType.AddressCheck)]
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


            if (!gp.PrivateIPEndPoint.Address.Equals(nn1.PrivateIPEndPoint.Address)
            || !nn1.PrivateIPEndPoint.Address.Equals(nn2.PrivateIPEndPoint.Address)
            || !nn2.PrivateIPEndPoint.Address.Equals(nn3.PrivateIPEndPoint.Address)
            )
            {
                natProperty.isIPRestricted = true;
            }

            if (gp.PublicIPEndPoint.Port != nn1.PublicIPEndPoint.Port
                 || nn1.PublicIPEndPoint.Port != nn2.PublicIPEndPoint.Port
                 || nn2.PublicIPEndPoint.Port != nn3.PublicIPEndPoint.Port)
            {
                natProperty.isPortRestricted = true;
            }

            // no nat
            if (gp.PublicIPEndPoint.Address.Equals(gp.PrivateIPEndPoint.Address)
            && nn1.PublicIPEndPoint.Address.Equals(nn1.PrivateIPEndPoint.Address)
            && nn2.PublicIPEndPoint.Address.Equals(nn2.PrivateIPEndPoint.Address)
            && nn3.PublicIPEndPoint.Address.Equals(nn3.PrivateIPEndPoint.Address))
            {
                if (gp.PublicIPEndPoint.Port == nn1.PublicIPEndPoint.Port &&
                nn1.PublicIPEndPoint.Port == nn2.PublicIPEndPoint.Port &&
                nn2.PublicIPEndPoint.Port == nn3.PublicIPEndPoint.Port)
                {
                    if (nn2.PublicIPEndPoint.Port == nn2.PrivateIPEndPoint.Port && nn3.PublicIPEndPoint.Port == nn3.PrivateIPEndPoint.Port)
                    {
                        // only if all condition satisfied this is NoNat
                        natProperty.NatType = NatType.NoNat;
                        return natProperty;
                    }
                }
            }

            // check if addresses are identical
            if (gp.PublicIPEndPoint.Address.Equals(gp.PrivateIPEndPoint.Address)
            && nn1.PublicIPEndPoint.Address.Equals(nn1.PublicIPEndPoint.Address)
            && nn2.PublicIPEndPoint.Address.Equals(nn2.PrivateIPEndPoint.Address)
            && nn3.PublicIPEndPoint.Address.Equals(nn3.PrivateIPEndPoint.Address)
            // check if public ports are identical
            && gp.PublicIPEndPoint.Port == nn1.PublicIPEndPoint.Port
            && nn1.PublicIPEndPoint.Port == nn2.PublicIPEndPoint.Port
            && nn2.PublicIPEndPoint.Port == nn3.PublicIPEndPoint.Port
            // check if the public port and private port is identical
            && nn2.PublicIPEndPoint.Port == nn2.PrivateIPEndPoint.Port
            && nn3.PublicIPEndPoint.Port == nn3.PrivateIPEndPoint.Port)
            {
                // actually, we can not determine the type of Cone NAT, because we do not send packet to client autonomously
                natProperty.NatType = NatType.FullCone;
                return natProperty;
            }


            // symmetric nat
            if (Math.Abs(nn2.PublicIPEndPoint.Port - nn2.PrivateIPEndPoint.Port) >= 1
            && Math.Abs(nn3.PublicIPEndPoint.Port - nn3.PrivateIPEndPoint.Port) >= 1)
            {
                natProperty.NatType = NatType.Symmetric;
                var tempIncrement1 = nn2.PublicIPEndPoint.Port - nn2.PrivateIPEndPoint.Port;
                var tempIncrement2 = nn3.PublicIPEndPoint.Port - nn3.PrivateIPEndPoint.Port;
                if (tempIncrement1 == tempIncrement2)
                {
                    natProperty.PortMapping = NatPortMappingScheme.Incremental;
                }
                else
                {
                    natProperty.PortMapping = NatPortMappingScheme.Mixed;
                    natProperty.PortIncrement.Add(tempIncrement1);
                    natProperty.PortIncrement.Add(tempIncrement2);
                }
                return natProperty;
            }

            return natProperty;
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
                case NatType.AddressRestrictedCone:
                case NatType.PortRestrictedCone:
                    return new IPEndPoint(initResults[NatPortType.NN1].PublicIPEndPoint.Address, initResults[NatPortType.NN1].PublicIPEndPoint.Port);
                case NatType.Symmetric:
                    switch (property.PortMapping)
                    {
                        case NatPortMappingScheme.Incremental:
                            return new IPEndPoint(initResults[NatPortType.NN1].PublicIPEndPoint.Address,
                                                    initResults[NatPortType.NN1].PublicIPEndPoint.Port + 1);
                        default:
                            return null;
                    }
                default:
                    return null;
            }
        }
    }
}
