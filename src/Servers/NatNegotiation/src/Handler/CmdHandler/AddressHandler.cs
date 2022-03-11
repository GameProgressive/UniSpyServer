using System;
using System.Net;
using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.Servers.NatNegotiation.Entity.Contract;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure;
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
            _result.PublicIPEndPoint = _client.Session.RemoteIPEndPoint;
        }
        protected override void ResponseConstruct()
        {
            _response = new InitResponse(_request, _result);
        }
        public static void DeterminIPandPortRestriction(Client client)
        {
            foreach (var req in client.Info.InitResults.Values)
            {
                if (client.Info.IsIPRestricted is null)
                {
                    if (req.PrivateIPEndPoint.Address.Equals(req.PrivateIPEndPoint.Address))
                    {
                        client.Info.IsIPRestricted = false;
                    }
                    else
                    {
                        client.Info.IsIPRestricted = true;
                    }
                }
                if (client.Info.IsPortRestricted is null)
                {
                    if (req.PrivateIPEndPoint.Port == req.PrivateIPEndPoint.Port)
                    {
                        client.Info.IsPortRestricted = false;
                    }
                    else
                    {
                        client.Info.IsPortRestricted = true;
                    }
                }
            }
        }
        public static void DetermineNatType(ClientInfo info)
        {
            // first we check if the nat is simplest situation
            if (info.IsIPRestricted == false
             && info.IsPortRestricted == false
             && info.InitResults[NatServerType.Map2].PublicIPEndPoint.Address == info.InitResults[NatServerType.Map2].PrivateIPEndPoint.Address)
            {
                // the perfect situation
                info.NatType = NatType.NoNat;
            }
            else if (info.InitResults[NatServerType.Map2].PublicIPEndPoint.Address == info.InitResults[NatServerType.Map2].PrivateIPEndPoint.Address)
            {
                // first we assume that the client's nat type is fullcone type
                info.NatType = NatType.FirewallOnly;
            }
            else
            {
                // var publicPort2 = info.InitResults[NatServerType.Map3].PublicIPEndPoint.Port;
                // var publicPort1 = info.InitResults[NatServerType.Map2].PrivateIPEndPoint.Port;
                if (info.IsIPRestricted == false
                 && info.IsPortRestricted == false
                 && info.InitResults[NatServerType.Map3].PublicIPEndPoint.Port != info.InitResults[NatServerType.Map2].PrivateIPEndPoint.Port)
                {
                    info.NatType = NatType.Symmetric;
                    info.Promiscuity = NatPromiscuity.Promiscuous;
                }
                else if (info.IsIPRestricted == true
                 && info.IsPortRestricted == false
                 && info.InitResults[NatServerType.Map3].PublicIPEndPoint.Port - info.InitResults[NatServerType.Map2].PrivateIPEndPoint.Port >= 1)
                {
                    info.NatType = NatType.Symmetric;
                    info.Promiscuity = NatPromiscuity.PortPromiscuous;
                }
                else if (info.IsIPRestricted == false
                    && info.IsPortRestricted == true
                    && info.InitResults[NatServerType.Map3].PublicIPEndPoint.Port - info.InitResults[NatServerType.Map2].PrivateIPEndPoint.Port >= 1)
                {
                    info.NatType = NatType.Symmetric;
                    info.Promiscuity = NatPromiscuity.IPPromiscuous;
                }
                else if (info.IsIPRestricted == true
                    && info.IsPortRestricted == true
                    && info.InitResults[NatServerType.Map3].PublicIPEndPoint.Port - info.InitResults[NatServerType.Map2].PrivateIPEndPoint.Port >= 1)
                {
                    info.NatType = NatType.Symmetric;
                    info.Promiscuity = NatPromiscuity.NotPromiscuous;
                }
                else if (info.IsPortRestricted == true)
                {
                    info.NatType = NatType.PortRestrictedCone;
                }
                else if (info.IsIPRestricted == true && info.IsPortRestricted == false)
                {
                    info.NatType = NatType.RestrictedCone;
                }
                else if (info.IsPortRestricted == false && info.IsPortRestricted == false)
                {
                    info.NatType = NatType.FullCone;
                }
                else
                {
                    info.NatType = NatType.Unknown;
                }

            }


        }

        public static void DetermineNatPortMapping(ClientInfo info)
        {
            var hasGamePort = info.InitResults[NatServerType.Map1A].PublicIPEndPoint.Port != 0;
            var hasServer3 = info.InitResults[NatServerType.Map1B].PublicIPEndPoint.Port != 0;
            if ((hasGamePort == false
                 || info.InitResults[NatServerType.Map1A].PublicIPEndPoint.Port == info.InitResults[NatServerType.Map1A].PrivateIPEndPoint.Port) &&
                 (info.InitResults[NatServerType.Map2].PublicIPEndPoint.Port == 0
                 || info.InitResults[NatServerType.Map2].PublicIPEndPoint.Port == info.InitResults[NatServerType.Map2].PrivateIPEndPoint.Port
                 && info.InitResults[NatServerType.Map3].PublicIPEndPoint.Port == info.InitResults[NatServerType.Map3].PrivateIPEndPoint.Port
                 && info.InitResults[NatServerType.Map1B].PublicIPEndPoint.Port == info.InitResults[NatServerType.Map1B].PrivateIPEndPoint.Port)
                 )
            {
                info.PortMappingScheme = NatPortMappingScheme.PrivateAsPublic;
            }
            else if (info.InitResults[NatServerType.Map2].PublicIPEndPoint.Port == info.InitResults[NatServerType.Map3].PublicIPEndPoint.Port
            && (info.InitResults[NatServerType.Map1B].PublicIPEndPoint.Port == 0
            || info.InitResults[NatServerType.Map3].PublicIPEndPoint.Port == info.InitResults[NatServerType.Map1A].PublicIPEndPoint.Port)
            )
            {
                info.PortMappingScheme = NatPortMappingScheme.ConsistentPort;
            }
            else if (
                (hasGamePort == false && (
                    info.InitResults[NatServerType.Map1A].PublicIPEndPoint.Port == info.InitResults[NatServerType.Map1A].PrivateIPEndPoint.Port)) && info.InitResults[NatServerType.Map3].PublicIPEndPoint.Port - info.InitResults[NatServerType.Map2].PublicIPEndPoint.Port == 1
            )
            {
                info.PortMappingScheme = NatPortMappingScheme.Mixed;
            }
            else if (info.InitResults[NatServerType.Map3].PublicIPEndPoint.Port - info.InitResults[NatServerType.Map2].PublicIPEndPoint.Port == 1)
            {
                info.PortMappingScheme = NatPortMappingScheme.Incremental;
            }
            else
            {
                info.PortMappingScheme = NatPortMappingScheme.Unrecognized;
            }
        }
        public static void DetermineNextAddress(ClientInfo info, IPEndPoint nextPublicIPEndPoint, IPEndPoint nextPrivateIPEndPoint)
        {
            IPEndPoint topEndPoint, bottomEndPoint, endPoint;
            var publicIP = BitConverter.ToInt32(info.InitResults[NatServerType.Map1A].PublicIPEndPoint.Address.GetAddressBytes());
            int i = -1;
            if (publicIP != 0)
            {
                topEndPoint = info.InitResults[NatServerType.Map1A].PublicIPEndPoint;
                nextPrivateIPEndPoint = info.InitResults[NatServerType.Map1A].PrivateIPEndPoint;
            }
            else
            {
                topEndPoint = info.InitResults[NatServerType.Map2].PublicIPEndPoint;
                nextPrivateIPEndPoint = info.InitResults[NatServerType.Map2].PrivateIPEndPoint;
                i++;
            }
            do
            {
                i++;
            }
            while (i + 1 < (info.InitResults.Count / 4) && info.InitResults[(NatServerType)i].PublicIPEndPoint.Port != 0);

            bottomEndPoint = info.InitResults[(NatServerType)i].PrivateIPEndPoint;
            endPoint = bottomEndPoint;
            switch ((NatPortMappingScheme)info.PortMappingScheme)
            {
                case NatPortMappingScheme.PrivateAsPublic:
                    endPoint = topEndPoint;
                    endPoint.Port = nextPrivateIPEndPoint.Port;
                    break;
                case NatPortMappingScheme.Unrecognized:
                case NatPortMappingScheme.ConsistentPort:
                    endPoint = topEndPoint;
                    break;
                case NatPortMappingScheme.Mixed:
                case NatPortMappingScheme.Incremental:
                    endPoint = bottomEndPoint;
                    endPoint.Port = bottomEndPoint.Port + 1;
                    break;
                default:
                    break;
            }
            nextPublicIPEndPoint = endPoint;
        }


    }
}