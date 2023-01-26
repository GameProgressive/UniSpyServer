using System.Collections.Generic;
using UniSpyServer.Servers.GameTrafficRelay.Entity;

namespace UniSpyServer.Servers.GameTrafficRelay.Interface
{
    public interface IStorageOperation
    {
        List<RelayServerInfo> GetAvaliableRelayServers();
    }
}