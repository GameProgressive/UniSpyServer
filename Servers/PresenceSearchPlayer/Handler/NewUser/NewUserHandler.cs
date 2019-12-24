using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.NewUser
{
    public class NewUserHandler : GPSPHandlerBase
    {
        private ushort _namespaceid;
        private uint _userid;
        private uint _profileid;
        public NewUserHandler(Dictionary<string, string> recv) : base(recv)
        {
        }

        protected override void CheckRequest(GPSPSession session)
        {
            base.CheckRequest(session);

            if (!_recv.ContainsKey("nick"))
            {
                _errorCode = GPErrorCode.Parse;
                return;
            }
            if (!_recv.ContainsKey("email") || !GameSpyUtils.IsEmailFormatCorrect(_recv["email"]))
            {
                _errorCode = GPErrorCode.Parse;
                return;
            }
            if (!_recv.ContainsKey("passenc"))
            {
                if (!_recv.ContainsKey("pass"))
                {
                    _errorCode = GPErrorCode.Parse;
                    return;
                }
            }

            if (_recv.ContainsKey("namespaceid"))
            {
                if (!ushort.TryParse(_recv["namespaceid"], out _namespaceid))
                {
                    _errorCode = GPErrorCode.Parse;
                }
            }
        }

        protected override void DataBaseOperation(GPSPSession session)
        {
            if (NewUserQuery.IsAccountExist(_recv["email"]))
            {
                if (!NewUserQuery.IsAccountCorrect(_recv["email"], _recv["passenc"], out _userid))
                {
                    _errorCode = GPErrorCode.NewUserBadPasswords;
                    return;
                }
            }
            else // Account not exist
            {
                NewUserQuery.CreateAccountOnUsersTable(_recv["email"], _recv["passenc"], out _userid);
            }

            if (_userid == 0)//error happend
            {
                _errorCode = GPErrorCode.DatabaseError;
                return;
            }

            NewUserQuery.FindOrCreateProfieOnProfileTable(_recv["nick"], _userid, out _profileid);

            if (!NewUserQuery.FindOrCreateSubProfileOnNamespaceTable(_recv["uniquenick"], _namespaceid, _profileid))
            {
                _errorCode = GPErrorCode.NewUserUniquenickInUse;
            }
            else
            {
                NewUserQuery.UpdateOtherInfo(_recv["uniquenick"], _namespaceid, _recv);
            }
        }

        protected override void ConstructResponse(GPSPSession session)
        {
            if (_errorCode != GPErrorCode.NoError)
            {
                _sendingBuffer = string.Format(@"\nur\{0}\final\", (uint)_errorCode);
            } 
            else
                _sendingBuffer = string.Format(@"\nur\0\pid\{0}\final\", _profileid);
        }
    }
}
