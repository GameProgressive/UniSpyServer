using System;
using System.Collections.Generic;
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


        public static NatProperty DetermineNatProperties(Dictionary<NatPortType, NatInitInfo> initResults)
        {
            NatType natType;
            NatPromiscuty promiscuity;
            NatPortMappingScheme map;
            promiscuity = NatPromiscuty.PromiscuityNotApplicable;
            if (initResults.Count != 4)
            {
                throw new NNException("We need 4 init results to determine the nat type.");
            }
            var isIPRestricted = false;
            var isPortRestricted = false;

            var gp = initResults[NatPortType.GP];
            var nn1 = initResults[NatPortType.NN1];
            var nn2 = initResults[NatPortType.NN2];
            var nn3 = initResults[NatPortType.NN3];
            if (!isIPRestricted && !isPortRestricted &&
                (nn2.PublicIPEndPoint.Address == nn2.PrivateIPEndPoint.Address))
            {
                natType = NatType.NoNat;
            }
            else if (nn2.PublicIPEndPoint.Address == nn2.PrivateIPEndPoint.Address)
            {
                natType = NatType.FirewallOnly;
            }
            else
            {
                if (!isIPRestricted && !isPortRestricted && (Math.Abs(nn3.PublicIPEndPoint.Port - nn2.PublicIPEndPoint.Port) >= 1))
                {
                    natType = NatType.Symmetric;
                    promiscuity = NatPromiscuty.Promiscuous;
                }
                else if (isIPRestricted && !isPortRestricted
                && Math.Abs(nn3.PublicIPEndPoint.Port - nn2.PublicIPEndPoint.Port) >= 1)
                {
                    natType = NatType.Symmetric;
                    promiscuity = NatPromiscuty.PortPromiscuous;
                }
                else if (!isIPRestricted && isPortRestricted
                    && Math.Abs(nn3.PublicIPEndPoint.Port - nn2.PublicIPEndPoint.Port) >= 1)
                {
                    natType = NatType.Symmetric;
                    promiscuity = NatPromiscuty.IpPromiscuous;
                }
                else if (isIPRestricted && isPortRestricted
                    && Math.Abs(nn3.PublicIPEndPoint.Port - nn2.PublicIPEndPoint.Port) >= 1)
                {
                    natType = NatType.Symmetric;
                    promiscuity = NatPromiscuty.NotPromiscuous;
                }
                else if (isPortRestricted)
                {
                    natType = NatType.PortRestrictedCone;
                }
                else if (isIPRestricted && !isPortRestricted)
                {
                    natType = NatType.AddressRestrictedCone;
                }
                else if (!isIPRestricted && !isPortRestricted)
                {
                    natType = NatType.FullCone;
                }
                else
                {
                    natType = NatType.Unknown;
                }
            }


            bool hasGPPacket = gp.PublicIPEndPoint.Port != 0;
            bool hasNN3 = nn3.PublicIPEndPoint.Port != 0;

            if ((!hasGPPacket || gp.PublicIPEndPoint.Port == gp.PrivateIPEndPoint.Port)
            && (nn1.PublicIPEndPoint.Port == nn1.PrivateIPEndPoint.Port)
            && (nn2.PublicIPEndPoint.Port == nn2.PublicIPEndPoint.Port)
            && (!hasNN3 || nn3.PublicIPEndPoint.Port == gp.PrivateIPEndPoint.Port))
            {
                map = NatPortMappingScheme.PrivateAsPublic;
            }
            else if (nn1.PublicIPEndPoint.Port == nn2.PublicIPEndPoint.Port
            && (gp.PublicIPEndPoint.Port == 0 || nn2.PublicIPEndPoint.Port == nn3.PublicIPEndPoint.Port))
            {
                map = NatPortMappingScheme.ConsistentPort;
            }
            else if ((hasGPPacket
            && (gp.PublicIPEndPoint.Port == gp.PrivateIPEndPoint.Port))
            && nn2.PublicIPEndPoint.Port == 1)
            {
                map = NatPortMappingScheme.Mixed;
            }
            else if (nn2.PublicIPEndPoint.Port - nn1.PublicIPEndPoint.Port == 1)
            {
                map = NatPortMappingScheme.Incremental;
            }
            else
            {
                map = NatPortMappingScheme.Unrecognized;
            }

            return new NatProperty()
            {
                NatType = natType,
                Promiscuity = promiscuity,
                PortMapping = map
            };
        }
        public static IPEndPoint GuessTargetAddress(NatProperty property, Dictionary<NatPortType, NatInitInfo> initResults, IPEndPoint natFailedEd = null)
        {
            // first try guess the target address
            // if (natFailedEd is null)
            // {
            //     if(nat.Type == NatType.NoNat)
            //     {
            //         return 
            //     }
            // }
            switch (property.NatType)
            {
                case NatType.NoNat:
                case NatType.FirewallOnly:
                    // private is public
                    return initResults[NatPortType.NN1].PrivateIPEndPoint;
                case NatType.FullCone:
                    // public address can accept all incoming message from any IP
                    return initResults[NatPortType.NN1].PublicIPEndPoint;
                case NatType.AddressRestrictedCone:
                case NatType.PortRestrictedCone:
                // // client must send packet to the negotiation target, otherwise the netotiation target's traffic will not pass the NAT
                // return initResults[NatPortType.NN1].PublicIPEndPoint;
                case NatType.Symmetric:
                    switch (property.PortMapping)
                    {
                        case NatPortMappingScheme.PrivateAsPublic:
                            return initResults[NatPortType.NN1].PublicIPEndPoint;
                        case NatPortMappingScheme.ConsistentPort:
                            throw new NotImplementedException();
                        case NatPortMappingScheme.Mixed:
                        case NatPortMappingScheme.Incremental:
                            return new IPEndPoint(initResults[NatPortType.NN1].PublicIPEndPoint.Address, initResults[NatPortType.NN1].PublicIPEndPoint.Port + 1);
                        case NatPortMappingScheme.Unrecognized:
                            break;
                    }
                    break;
            }
            return initResults[NatPortType.NN1].PublicIPEndPoint;

        }
    }
}
