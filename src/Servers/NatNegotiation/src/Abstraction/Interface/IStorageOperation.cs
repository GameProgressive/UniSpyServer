using System;
using System.Collections.Generic;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Redis;

namespace UniSpyServer.Servers.NatNegotiation.Abstraction.Interface
{
    public interface IStorageOperation
    {
        void UpdateInitInfo(NatAddressInfo info);
        int CountInitInfo(uint cookie, byte version);
        List<NatAddressInfo> GetInitInfos(Guid serverId, uint cookie);
        void RemoveInitInfo(NatAddressInfo info);
    }
}