using System.Collections.Generic;
using UniSpy.Server.GameTrafficRelay.Entity;

namespace UniSpy.Server.GameTrafficRelay.Interface
{
    public interface IStorageOperation
    {
        List<RelayServerInfo> GetAvaliableRelayServers();
    }
}