using System.Collections.Generic;
using System.Net;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Redis;
using UniSpyServer.Servers.NatNegotiation.Handler.CmdHandler;
using Xunit;

namespace UniSpyServer.Servers.NatNegotiation.Test
{
    public class NatDetectionTest
    {
        [Fact]
        public void PublicIPTest()
        {
            var gp = new NatInitInfo()
            {
                PrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            };
            var nn1 = new NatInitInfo()
            {
                PrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            };
            var nn2 = new NatInitInfo()
            {
                PrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            };
            var nn3 = new NatInitInfo()
            {
                PrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            };
            var initResults = new Dictionary<NatPortType, NatInitInfo>()
            {
                { NatPortType.GP, gp },
                { NatPortType.NN1, nn1 },
                { NatPortType.NN2, nn2 },
                { NatPortType.NN3, nn3 }
            };

            var prop = AddressCheckHandler.DetermineNatType(initResults);

            Assert.Equal(prop.NatType, NatType.NoNat);
        }
        [Fact]
        public void FullConeTest()
        {
            var gp = new NatInitInfo()
            {
                PrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            };
            var nn1 = new NatInitInfo()
            {
                PrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            };
            var nn2 = new NatInitInfo()
            {
                PrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            };
            var nn3 = new NatInitInfo()
            {
                PrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            };
            var initResults = new Dictionary<NatPortType, NatInitInfo>()
            {
                { NatPortType.GP, gp },
                { NatPortType.NN1, nn1 },
                { NatPortType.NN2, nn2 },
                { NatPortType.NN3, nn3 }
            };
            var prop = AddressCheckHandler.DetermineNatType(initResults);
            Assert.Equal(prop.NatType, NatType.FullCone);

        }

        [Fact]
        public void SymetricTest()
        {
            var gp = new NatInitInfo()
            {
                PrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            };
            var nn1 = new NatInitInfo()
            {
                PrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.2"), 81)
            };
            var nn2 = new NatInitInfo()
            {
                PrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.3"), 82)
            };
            var nn3 = new NatInitInfo()
            {
                PrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.4"), 83)
            };
            var initResults = new Dictionary<NatPortType, NatInitInfo>()
            {
                { NatPortType.GP, gp },
                { NatPortType.NN1, nn1 },
                { NatPortType.NN2, nn2 },
                { NatPortType.NN3, nn3 }
            };

            var prop = AddressCheckHandler.DetermineNatType(initResults);
            Assert.Equal(prop.NatType, NatType.Symmetric);

        }
        [Fact]
        public void RestrictedConeTest()
        {
            var gp = new NatInitInfo()
            {
                PrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            };
            var nn1 = new NatInitInfo()
            {
                PrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.2"), 80)
            };
            var nn2 = new NatInitInfo()
            {
                PrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.3"), 80)
            };
            var nn3 = new NatInitInfo()
            {
                PrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.4"), 80)
            };
            var initResults = new Dictionary<NatPortType, NatInitInfo>()
            {
                { NatPortType.GP, gp },
                { NatPortType.NN1, nn1 },
                { NatPortType.NN2, nn2 },
                { NatPortType.NN3, nn3 }
            };

            var prop = AddressCheckHandler.DetermineNatType(initResults);
            Assert.Equal(prop.NatType, NatType.AddressRestrictedCone);
        }
        [Fact]
        public void PortRestrictedConeTest()
        {
            var gp = new NatInitInfo()
            {
                PrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            };
            var nn1 = new NatInitInfo()
            {
                PrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            };
            var nn2 = new NatInitInfo()
            {
                PrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 81)
            };
            var nn3 = new NatInitInfo()
            {
                PrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 82)
            };
            var initResults = new Dictionary<NatPortType, NatInitInfo>()
            {
                { NatPortType.GP, gp },
                { NatPortType.NN1, nn1 },
                { NatPortType.NN2, nn2 },
                { NatPortType.NN3, nn3 }
            };
            var prop = AddressCheckHandler.DetermineNatType(initResults);
            Assert.Equal(prop.NatType, NatType.PortRestrictedCone);
        }
        [Fact]
        public void JuliusNetworkTest()
        {
            var gp = new NatInitInfo()
            {
                PrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.60"), 0),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("91.52.105.210"), 51520)
            };
            var nn1 = new NatInitInfo()
            {
                PrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 0),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("91.52.105.210"), 51521)
            };
            var nn2 = new NatInitInfo()
            {
                PrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.60"), 49831),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("91.52.105.210"), 49832)
            };
            var nn3 = new NatInitInfo()
            {
                PrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.60"), 49831),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("91.52.105.210"), 49832)
            };
            var initResults = new Dictionary<NatPortType, NatInitInfo>()
            {
                { NatPortType.GP, gp },
                { NatPortType.NN1, nn1 },
                { NatPortType.NN2, nn2 },
                { NatPortType.NN3, nn3 }
            };
            var prop = AddressCheckHandler.DetermineNatType(initResults);
            Assert.Equal(prop.NatType, NatType.Symmetric);
        }
    }
}