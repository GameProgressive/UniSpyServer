using System.ComponentModel;
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
            var initResults = new Dictionary<NatPortType, NatInitInfo>();

            var initInfo = new NatInitInfo();
            initInfo.AddressInfos.TryAdd(NatPortType.GP, new NatAddressInfo()
            {
                PublicIPEndPoint = IPEndPoint.Parse("10.0.0.1:1"),
                PrivateIPEndPoint = IPEndPoint.Parse("10.0.0.1:1")
            });
            initInfo.AddressInfos.TryAdd(NatPortType.NN1, new NatAddressInfo()
            {
                PublicIPEndPoint = IPEndPoint.Parse("10.0.0.1:2"),
                PrivateIPEndPoint = IPEndPoint.Parse("10.0.0.1:2")
            });
            initInfo.AddressInfos.TryAdd(NatPortType.NN2, new NatAddressInfo()
            {
                PublicIPEndPoint = IPEndPoint.Parse("10.0.0.1:2"),
                PrivateIPEndPoint = IPEndPoint.Parse("10.0.0.1:2")
            });
            initInfo.AddressInfos.TryAdd(NatPortType.NN3, new NatAddressInfo()
            {
                PublicIPEndPoint = IPEndPoint.Parse("10.0.0.1:2"),
                PrivateIPEndPoint = IPEndPoint.Parse("10.0.0.1:2")
            });

            var prop = AddressCheckHandler.DetermineNatType(initInfo);

            Assert.Equal(NatType.NoNat, prop.NatType);
        }
        [Fact]
        public void FullConeTest()
        {
            var initResults = new Dictionary<NatPortType, NatInitInfo>();

            var initInfo = new NatInitInfo();
            initInfo.AddressInfos.TryAdd(NatPortType.GP, new NatAddressInfo()
            {
                PublicIPEndPoint = IPEndPoint.Parse("10.0.0.1:1"),
                PrivateIPEndPoint = IPEndPoint.Parse("192.168.1.1:1")
            });
            initInfo.AddressInfos.TryAdd(NatPortType.NN1, new NatAddressInfo()
            {
                PublicIPEndPoint = IPEndPoint.Parse("10.0.0.1:2"),
                PrivateIPEndPoint = IPEndPoint.Parse("192.168.1.1:0")
            });
            initInfo.AddressInfos.TryAdd(NatPortType.NN2, new NatAddressInfo()
            {
                PublicIPEndPoint = IPEndPoint.Parse("10.0.0.1:2"),
                PrivateIPEndPoint = IPEndPoint.Parse("192.168.1.1:2")
            });
            initInfo.AddressInfos.TryAdd(NatPortType.NN3, new NatAddressInfo()
            {
                PublicIPEndPoint = IPEndPoint.Parse("10.0.0.1:2"),
                PrivateIPEndPoint = IPEndPoint.Parse("192.168.1.1:2")
            });

            var prop = AddressCheckHandler.DetermineNatType(initInfo);
            Assert.Equal(NatType.FullCone, prop.NatType);

        }

        [Fact]
        public void SymetricTest()
        {
            var initInfo = new NatInitInfo();
            initInfo.AddressInfos.TryAdd(NatPortType.GP, new NatAddressInfo()
            {
                PublicIPEndPoint = IPEndPoint.Parse("10.0.0.1:1"),
                PrivateIPEndPoint = IPEndPoint.Parse("192.168.1.1:1")
            });
            initInfo.AddressInfos.TryAdd(NatPortType.NN1, new NatAddressInfo()
            {
                PublicIPEndPoint = IPEndPoint.Parse("10.0.0.1:2"),
                PrivateIPEndPoint = IPEndPoint.Parse("192.168.1.1:2")
            });
            initInfo.AddressInfos.TryAdd(NatPortType.NN2, new NatAddressInfo()
            {
                PublicIPEndPoint = IPEndPoint.Parse("10.0.0.1:3"),
                PrivateIPEndPoint = IPEndPoint.Parse("192.168.1.1:2")
            });
            initInfo.AddressInfos.TryAdd(NatPortType.NN3, new NatAddressInfo()
            {
                PublicIPEndPoint = IPEndPoint.Parse("10.0.0.1:4"),
                PrivateIPEndPoint = IPEndPoint.Parse("192.168.1.1:2")
            });

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
            var initInfo = new NatInitInfo();
            initInfo.AddressInfos.TryAdd(NatPortType.GP, new NatAddressInfo()
            {
                PublicIPEndPoint = IPEndPoint.Parse("91.52.105.210:51520"),
                PrivateIPEndPoint = IPEndPoint.Parse("192.168.0.60:0")
            });
            initInfo.AddressInfos.TryAdd(NatPortType.NN1, new NatAddressInfo()
            {
                PublicIPEndPoint = IPEndPoint.Parse("91.52.105.210:51521"),
                PrivateIPEndPoint = IPEndPoint.Parse("192.168.0.60:0")
            });
            initInfo.AddressInfos.TryAdd(NatPortType.NN2, new NatAddressInfo()
            {
                PublicIPEndPoint = IPEndPoint.Parse("91.52.105.210:49832"),
                PrivateIPEndPoint = IPEndPoint.Parse("192.168.0.60:49832")
            });
            initInfo.AddressInfos.TryAdd(NatPortType.NN3, new NatAddressInfo()
            {
                PublicIPEndPoint = IPEndPoint.Parse("91.52.105.210:49832"),
                PrivateIPEndPoint = IPEndPoint.Parse("192.168.0.60:49832")
            });

            var prop = AddressCheckHandler.DetermineNatType(initInfo);
            Assert.Equal(NatType.Symmetric, prop.NatType);
        }
    }
}