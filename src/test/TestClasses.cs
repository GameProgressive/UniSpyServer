using System.Collections.Generic;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Test
{
    public static class TestClasses
    {
        public static void ServersSimulation(List<KeyValuePair<string, byte[]>> requests, Dictionary<string, IClient> clients)
        {
            foreach (var req in requests)
            {
                ((ITestClient)clients[req.Key]).TestReceived(req.Value);
            }
        }
    }
}