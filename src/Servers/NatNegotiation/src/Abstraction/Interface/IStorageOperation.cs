using System;
using System.Collections.Generic;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Redis;

namespace UniSpyServer.Servers.NatNegotiation.Abstraction.Interface
{
    public interface IStorageOperation
    {
        void UpdateInitInfo(NatInitInfo info);
        int CountInitInfo(uint cookie, byte version);
        List<NatInitInfo> GetInitInfos(Guid serverId, uint cookie);
        void RemoveInitInfo(NatInitInfo info);
    }
}