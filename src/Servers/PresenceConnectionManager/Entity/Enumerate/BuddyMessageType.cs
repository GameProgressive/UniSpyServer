using System;
namespace PresenceConnectionManager.Entity.Enumerate
{
    public enum BuddyMessageType
    {
        #region response
        //BM status
        BmMessage = 1,
        BmRquest = 2,
        BmReply = 3, // only used on the backend
        BmAuth = 4,
        BmUTM = 5,
        BmRevoke = 6,  // remote buddy removed from local list
        BmStatus = 100,
        BmInvite = 101,
        BmPing = 102,
        #endregion
        #region request
        BmPong = 103,
        BmKeysRequest = 104,
        BmKeysReply = 105,
        BmFileSendRequest = 200,
        BmFileSendReply = 201,
        BmFileBegin = 202,
        BmFileEnd = 203,
        BmFileData = 204,
        BmFile_SKIP = 205,
        BmFileTransferThrottle = 206,
        BmFileTransferCancel = 207,
        BmFileTransferKeepAlive = 208,
        #endregion
    }
}
