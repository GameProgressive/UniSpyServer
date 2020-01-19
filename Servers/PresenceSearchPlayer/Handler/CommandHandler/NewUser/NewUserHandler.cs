using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.CommandHandler.NewUser
{
    public class NewUserHandler : GPSPHandlerBase
    {
        private uint _userid;
        private uint _profileid;
        private string _uniquenick = "";
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
                _errorCode = GPErrorCode.Parse;
                return;
            }

            if (_recv.ContainsKey("uniquenick"))
            {
                _uniquenick = _recv["uniquenick"];
            }
        }

        protected override void DataBaseOperation(GPSPSession session)
        {
            //bool IsEmail=false;
            //bool IsEmailPasswordCorrect = false;
            //bool IsNickExist = false;
            //bool IsUniquenickExist = false;
            //bool NewUserWithUniquenick =false;
            //bool NewUserWithEmail = false;

            //if(_recv.ContainsKey("uniquenick"))
            //{
            //    NewUserWithUniquenick = true;
            //}
            //else
            //{
            //    NewUserWithEmail = true;
            //}


            //if (NewUserWithEmail)
            //{
            //    if (NewUserQuery.IsAccountExist(_recv["email"]))
            //    {
            //        if (!NewUserQuery.IsAccountCorrect(_recv["email"], _recv["passenc"], out _userid))
            //        {
            //            _errorCode = GPErrorCode.NewUserBadPasswords;
            //            return;
            //        }
            //        else
            //        {
            //            if (IsNickExist())
            //            { }
            //        }
            //    }
            //}
            //else



            //if (IsAccountExist)
            //{
            //    if (IsAccountCorrect)
            //    {
            //        if (IsNickExist)
            //        {
            //            if (IsUniquenickExist)
            //            {
            //                _errorCode = GPErrorCode.NewUserUniquenickInUse;
            //            }
            //            else
            //            {

            //            }
            //        }
            //        else
            //        {
            //            //RegisterNickAndUniquenick();
            //        }
            //    }
            //}
            //else
            //{
            //    //RegisterNewAcount();
            //}              

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

            NewUserQuery.FindOrCreateProfileOnProfileTable(_recv["nick"], _userid, out _profileid);

            if (!NewUserQuery.FindOrCreateSubProfileOnNamespaceTable(_uniquenick, _namespaceid, _profileid))
            {
                _errorCode = GPErrorCode.NewUserUniquenickInUse;
            }
            else
            {
                NewUserQuery.UpdateOtherInfo(_uniquenick, _namespaceid, _recv);
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
