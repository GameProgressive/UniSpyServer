using System.Collections.Generic;
using UniSpyServer.Servers.GameStatus.Entity.Enumerate;

namespace UniSpyServer.Servers.GameStatus.Abstraction.Interface
{
    public interface IStorageOperation
    {
        void CreateNewGameData();
        void CreateNewPlayerData(Dictionary<string, string> playerData);
        void UpdatePlayerData(int profileId, PersistStorageType storageType, int dataIndex, Dictionary<string, string> data);
        int GetProfileId(string token);
        int GetProfileId(string cdKey, string nickName);
        int GetProfileId(int profileId);
        Dictionary<string, string> GetPlayerData(int profileId, PersistStorageType storageType, int dataIndex);
    }
}