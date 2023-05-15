namespace UniSpy.Server.Chat.Abstraction.Interface
{
    public interface IStorageOperation
    {
        (int userId, int profileId, bool emailVerified, bool banned) NickAndEmailLogin(string nickName, string email, string passwordHash);
        (int userId, int profileId, bool emailVerified, bool banned) UniqueNickLogin(string uniqueNick, int namespaceId);
    }
}