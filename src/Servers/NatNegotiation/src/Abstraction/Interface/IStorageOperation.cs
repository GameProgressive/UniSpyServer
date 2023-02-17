using System;
using System.Collections.Generic;
using UniSpy.Server.NatNegotiation.Entity.Structure.Redis;

namespace UniSpy.Server.NatNegotiation.Abstraction.Interface
{
    public interface IStorageOperation
    {
        void UpdateInitInfo(NatAddressInfo info);
        int CountInitInfo(uint cookie, byte version);
        List<NatAddressInfo> GetInitInfos(Guid serverId, uint cookie);
        void RemoveInitInfo(NatAddressInfo info);
        void UpdateNatFailInfo(NatNegotiation.Entity.Structure.Redis.Fail.NatFailInfo info);
        int GetNatFailInfo(NatNegotiation.Entity.Structure.Redis.Fail.NatFailInfo info);
    }
}