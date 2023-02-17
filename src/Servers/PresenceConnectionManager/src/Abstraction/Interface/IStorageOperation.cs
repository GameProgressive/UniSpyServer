using System.Collections.Generic;
using UniSpy.Server.PresenceConnectionManager.Entity.Structure.Result;
using UniSpy.Server.Core.Database.DatabaseModel;

namespace UniSpy.Server.PresenceConnectionManager.Abstraction.Interface
{
    public interface IStorageOperation
    {
        List<int> GetBlockedProfileIds(int profileId, int namespaceId);
        List<int> GetFriendProfileIds(int profileId, int namespaceId);
        void DeleteFriendByProfileId(int profileId, int targetId, int namespaceId);
        (User, Profile, Subprofile) GetUsersInfos(string email, string nickName);
        (User, Profile, Subprofile) GetUsersInfos(string uniqueNick, int namespaceId);
        (User, Profile, Subprofile) GetUsersInfos(string authToken, int partnerId, int namespaceId);
        void UpdateBlockInfo(int targetId, int profileId, int namespaceId);
        void UpdateFriendInfo(int targetId, int profileId, int namespaceId);
        void UpdateProfileInfo(Profile profile);
        GetProfileDataModel GetProfileInfos(int profileId, int namespaceId);
        void UpdateUniqueNick(int subProfileId, string uniqueNick);
        void UpdateNickName(int profileId, string oldNick, string newNick);
        void AddNickName(int userId, int profileId, string newNick);
        void UpdateSubProfileInfo(Subprofile subprofile);
        bool IsEmailExist(string email);
    }
}