using System.Collections.Generic;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Test
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