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
            initResults.Add(NatPortType.GP, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 0),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            });
            initResults.Add(NatPortType.NN1, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 0),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            });
            initResults.Add(NatPortType.NN2, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 81),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 81)
            });
            initResults.Add(NatPortType.NN3, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 81),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 81)
            });
            var prop = AddressCheckHandler.DetermineNatType(initResults);

            Assert.Equal(NatType.NoNat, prop.NatType);
        }
        [Fact]
        public void FullConeTest()
        {

            var initResults = new Dictionary<NatPortType, NatInitInfo>();
            initResults.Add(NatPortType.GP, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 0),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            });
            initResults.Add(NatPortType.NN1, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 81),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            });
            initResults.Add(NatPortType.NN2, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 81),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            });
            initResults.Add(NatPortType.NN3, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 81),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            });

            var prop = AddressCheckHandler.DetermineNatType(initResults);
            Assert.Equal(NatType.FullCone, prop.NatType);

        }

        [Fact]
        public void SymetricTest()
        {
            var initResults = new Dictionary<NatPortType, NatInitInfo>();
            initResults.Add(NatPortType.GP, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 0),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            });
            initResults.Add(NatPortType.NN1, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 0),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 81)
            });
            initResults.Add(NatPortType.NN2, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 82)
            });
            initResults.Add(NatPortType.NN3, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 83)
            });

            var prop = AddressCheckHandler.DetermineNatType(initResults);
            Assert.Equal(NatType.Symmetric, prop.NatType);

        }
        [Fact]
        /// <summary>
        /// Because GameSpy natneg did not send message to client so we can not tell it is restricted cone
        /// </summary>
        public void RestrictedConeTest()
        {
            var initResults = new Dictionary<NatPortType, NatInitInfo>();
            initResults.Add(NatPortType.GP, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 0),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            });
            initResults.Add(NatPortType.NN1, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 0),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            });
            initResults.Add(NatPortType.NN2, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 81),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            });
            initResults.Add(NatPortType.NN3, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 82),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            });

            var prop = AddressCheckHandler.DetermineNatType(initResults);
            Assert.Equal(NatType.FullCone, prop.NatType);
        }
        [Fact]
        /// <summary>
        /// Because Natneg did not send message to client, so we can not tell if it is a port restricted cone
        /// </summary>
        public void PortRestrictedConeTest()
        {
            var initResults = new Dictionary<NatPortType, NatInitInfo>();
            initResults.Add(NatPortType.GP, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 0),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            });
            initResults.Add(NatPortType.NN1, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 0),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            });
            initResults.Add(NatPortType.NN2, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 81),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            });
            initResults.Add(NatPortType.NN3, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 82),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            });
            var prop = AddressCheckHandler.DetermineNatType(initResults);
            Assert.Equal(NatType.FullCone, prop.NatType);
        }

        [Fact]
        public void LanTest()
        {
            # region same ip test
            var clientPackets = new Dictionary<NatPortType, NatInitInfo>()
            {
                { NatPortType.GP, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 0),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            } },
                { NatPortType.NN1, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 0),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            } },
                { NatPortType.NN2, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 81)
            } },
                { NatPortType.NN3, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 82)
            } }
            };
            var serverPackets = new Dictionary<NatPortType, NatInitInfo>()
            {
                { NatPortType.GP, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 0),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            } },
                { NatPortType.NN1, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 0),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            } },
                { NatPortType.NN2, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 81),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 81)
            } },
                { NatPortType.NN3, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 81),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 81)
            } }
            };
            // Given
            bool result = AddressCheckHandler.IsInSameLan(clientPackets, serverPackets);
            Assert.True(result);

            #endregion
            clientPackets = new Dictionary<NatPortType, NatInitInfo>()
            {
                { NatPortType.GP, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 0),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            } },
                { NatPortType.NN1, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 0),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            } },
                { NatPortType.NN2, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 81)
            } },
                { NatPortType.NN3, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 80),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 82)
            } }
            };
            serverPackets = new Dictionary<NatPortType, NatInitInfo>()
            {
                { NatPortType.GP, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.2"), 0),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            } },
                { NatPortType.NN1, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.2"), 0),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 80)
            } },
                { NatPortType.NN2, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 83),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 83)
            } },
                { NatPortType.NN3, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 84),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.1"), 84)
            } }
            };
            result = AddressCheckHandler.IsInSameLan(clientPackets, serverPackets);
            Assert.True(result);
        }
        [Fact]
        public void JuliusNetworkTest()
        {
            var initResults = new Dictionary<NatPortType, NatInitInfo>()
            {
                { NatPortType.GP, new NatInitInfo()
            {
                 GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.60"), 0),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("91.52.105.210"), 51520)
            } },
                { NatPortType.NN1, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 0),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("91.52.105.210"), 51521)
            } },
                { NatPortType.NN2, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.60"), 49832),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("91.52.105.210"), 49832)
            } },
                { NatPortType.NN3, new NatInitInfo()
            {
                GPPrivateIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.60"), 49832),
                PublicIPEndPoint = new IPEndPoint(IPAddress.Parse("91.52.105.210"), 49832)
            } }
            };
            
            var prop = AddressCheckHandler.DetermineNatType(initResults);
            Assert.Equal(NatType.Symmetric, prop.NatType);
        }
    }
}