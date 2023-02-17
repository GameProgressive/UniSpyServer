using System.Collections.Generic;
using System.Net;
using UniSpy.Server.NatNegotiation.Entity.Enumerate;
using UniSpy.Server.NatNegotiation.Entity.Structure.Redis;
using UniSpy.Server.NatNegotiation.Handler.CmdHandler;
using Xunit;

namespace UniSpy.Server.NatNegotiation.Test
{
    public class NatDetectionTest
    {
        [Fact]
        public void PublicIPTest()
        {
            var list = new List<NatAddressInfo>()
            {
                new NatAddressInfo()
            {
                PortType = NatPortType.GP,
                ClientIndex = NatClientIndex.GameClient,
                Cookie = 123,
                Version=3,
                PublicIPEndPoint = IPEndPoint.Parse("10.0.0.1:1"),
                PrivateIPEndPoint = IPEndPoint.Parse("10.0.0.1:0")
            },
            new NatAddressInfo()
            {
                PortType = NatPortType.NN1,
                ClientIndex = NatClientIndex.GameClient,
                Cookie = 123,
                Version=3,
                PublicIPEndPoint = IPEndPoint.Parse("10.0.0.1:2"),
                PrivateIPEndPoint = IPEndPoint.Parse("10.0.0.1:0")
            },
            new NatAddressInfo()
            {
                PortType = NatPortType.NN2,
                ClientIndex = NatClientIndex.GameClient,
                Cookie = 123,
                Version=3,
                PublicIPEndPoint = IPEndPoint.Parse("10.0.0.1:2"),
                PrivateIPEndPoint = IPEndPoint.Parse("10.0.0.1:2")
            },
            new NatAddressInfo()
            {
                PortType = NatPortType.NN3,
                ClientIndex = NatClientIndex.GameClient,
                Cookie = 123,
                Version=3,
                PublicIPEndPoint = IPEndPoint.Parse("10.0.0.1:2"),
                PrivateIPEndPoint = IPEndPoint.Parse("10.0.0.1:2")
            }
            };
            var initInfo = new NatInitInfo(list);
            var prop = AddressCheckHandler.DetermineNatType(initInfo);

            Assert.Equal(NatType.NoNat, prop.NatType);
        }
        [Fact]
        public void FullConeTest()
        {
            var list = new List<NatAddressInfo>()
            {
                new NatAddressInfo()
            {
                PortType = NatPortType.GP,
                ClientIndex = NatClientIndex.GameClient,
                Cookie = 123,
                Version=3,
                PublicIPEndPoint = IPEndPoint.Parse("10.0.0.1:1"),
                PrivateIPEndPoint = IPEndPoint.Parse("192.168.1.1:0")
            },
            new NatAddressInfo()
            {
                PortType = NatPortType.NN1,
                ClientIndex = NatClientIndex.GameClient,
                Cookie = 123,
                Version=3,
                PublicIPEndPoint = IPEndPoint.Parse("10.0.0.1:2"),
                PrivateIPEndPoint = IPEndPoint.Parse("192.168.1.1:0")
            },
            new NatAddressInfo()
            {
                PortType = NatPortType.NN2,
                ClientIndex = NatClientIndex.GameClient,
                Cookie = 123,
                Version=3,
                PublicIPEndPoint = IPEndPoint.Parse("10.0.0.1:2"),
                PrivateIPEndPoint = IPEndPoint.Parse("192.168.1.1:2")
            },
            new NatAddressInfo()
            {
                PortType = NatPortType.NN3,
                ClientIndex = NatClientIndex.GameClient,
                Cookie = 123,
                Version=3,
                PublicIPEndPoint = IPEndPoint.Parse("10.0.0.1:2"),
                PrivateIPEndPoint = IPEndPoint.Parse("192.168.1.1:2")
            }
            };
            var initInfo = new NatInitInfo(list);
            var prop = AddressCheckHandler.DetermineNatType(initInfo);
            Assert.Equal(NatType.FullCone, prop.NatType);

        }

        [Fact]
        public void SymetricTest()
        {
            var list = new List<NatAddressInfo>()
            {
                new NatAddressInfo()
            {
                PortType = NatPortType.GP,
                ClientIndex = NatClientIndex.GameClient,
                Cookie = 123,
                Version=3,
                PublicIPEndPoint = IPEndPoint.Parse("10.0.0.1:1"),
                PrivateIPEndPoint = IPEndPoint.Parse("192.168.1.1:1")
            },
            new NatAddressInfo()
            {
                PortType = NatPortType.NN1,
                ClientIndex = NatClientIndex.GameClient,
                Cookie = 123,
                Version=3,
                PublicIPEndPoint = IPEndPoint.Parse("10.0.0.1:2"),
                PrivateIPEndPoint = IPEndPoint.Parse("192.168.1.1:2")
            },
            new NatAddressInfo()
            {
                PortType = NatPortType.NN2,
                ClientIndex = NatClientIndex.GameClient,
                Cookie = 123,
                Version=3,
                PublicIPEndPoint = IPEndPoint.Parse("10.0.0.1:3"),
                PrivateIPEndPoint = IPEndPoint.Parse("192.168.1.1:2")
            },
            new NatAddressInfo()
            {
                PortType = NatPortType.NN3,
                ClientIndex = NatClientIndex.GameClient,
                Cookie = 123,
                Version=3,
                PublicIPEndPoint = IPEndPoint.Parse("10.0.0.1:4"),
                PrivateIPEndPoint = IPEndPoint.Parse("192.168.1.1:2")
            }
            };
            var initInfo = new NatInitInfo(list);
            var prop = AddressCheckHandler.DetermineNatType(initInfo);
            Assert.Equal(NatType.Symmetric, prop.NatType);

        }
        [Fact]
        public void NatStrategyTest()
        {

        }
        [Fact]
        public void JuliusNetworkTest()
        {
            var list = new List<NatAddressInfo>()
            {
                new NatAddressInfo()
            {
                PortType = NatPortType.GP,
                ClientIndex = NatClientIndex.GameClient,
                Cookie = 123,
                Version=3,
                PublicIPEndPoint = IPEndPoint.Parse("91.52.105.210:51520"),
                PrivateIPEndPoint = IPEndPoint.Parse("192.168.0.60:0")
            },
            new NatAddressInfo()
            {
                PortType = NatPortType.NN1,
                ClientIndex = NatClientIndex.GameClient,
                Cookie = 123,
                Version=3,
                PublicIPEndPoint = IPEndPoint.Parse("91.52.105.210:51521"),
                PrivateIPEndPoint = IPEndPoint.Parse("192.168.0.60:0")
            },new NatAddressInfo()
            {
                PortType = NatPortType.NN2,
                ClientIndex = NatClientIndex.GameClient,
                Cookie = 123,
                Version=3,
                PublicIPEndPoint = IPEndPoint.Parse("91.52.105.210:49832"),
                PrivateIPEndPoint = IPEndPoint.Parse("192.168.0.60:49832")
            },new NatAddressInfo()
            {
                PortType = NatPortType.NN3,
                ClientIndex = NatClientIndex.GameClient,
                Cookie = 123,
                Version=3,
                PublicIPEndPoint = IPEndPoint.Parse("91.52.105.210:49832"),
                PrivateIPEndPoint = IPEndPoint.Parse("192.168.0.60:49832")
            },
            };
            var initInfo = new NatInitInfo(list);

            var prop = AddressCheckHandler.DetermineNatType(initInfo);
            Assert.Equal(NatType.Symmetric, prop.NatType);
        }
    }
}