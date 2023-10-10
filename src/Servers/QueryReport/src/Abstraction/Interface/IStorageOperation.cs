using System.Collections.Generic;
using UniSpy.Server.Chat.Aggregate;

namespace UniSpy.Server.QueryReport.Abstraction.Interface
{
    public interface IStorageOperation
    {
        List<Channel> GetPeerStagingChannel(string gameName, int groupId);
        List<Channel> GetPeerGroupChannel(int groupId);
    }
}