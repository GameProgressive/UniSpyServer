using System.Collections.Generic;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Abstraction.Interface
{
    public interface IStorageOperation
    {
        int? GetProfileId(string email, string password, string nickName, int partnerId);
        bool VerifyEmail(string email);
        bool VerifyEmailAndPassword(string email, string password);
        void AddUser(User user);
        void AddProfile(Profile profile);
        void AddSubProfile(Subprofile subprofile);
        void UpdateProfile(Profile profile);
        void UpdateSubProfile(Subprofile subProfile);
        User GetUser(string email);
        Profile GetProfile(int userId, string nickName);
        Subprofile GetSubProfile(int profileId, int namespaceId, int productId);
        List<NicksDataModel> GetAllNickAndUniqueNick(string email, string password, int namespaceId);
        List<OthersDatabaseModel> GetFriendsInfo(int profileId, int namespaceId, string gameName);
        List<OthersListDatabaseModel> GetMatchedProfileIdInfos(List<int> profileIds, int namespaceId);
        List<SearchDataBaseModel> GetMatchedInfosByNick(string nickName);
        List<SearchDataBaseModel> GetMatchedInfosByEmail(string email);
        List<SearchDataBaseModel> GetMatchedInfosByNickAndEmail(string nickName, string email);
        List<SearchDataBaseModel> GetMatchedInfosByUniqueNickAndNamespaceId(string uniqueNick, int namespaceId);
        List<SearchUniqueDatabaseModel> GetMatchedInfosByNamespaceId(List<int> namespaceIds, string uniqueNick);
        bool IsUniqueNickExist(string uniqueNick, int namespaceId, string gameName);
        bool IsEmailExist(string email);
    }
}