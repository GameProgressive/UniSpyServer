using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exception.NewUser
{
    public class GPNewUserBadPasswordException : GPNewUserException
    {
        public GPNewUserBadPasswordException() : base("Password is invalid!", GPErrorCode.NewUserBadPasswords)
        {
        }

        public GPNewUserBadPasswordException(string message) : base(message, GPErrorCode.NewUserBadPasswords)
        {
        }

        public GPNewUserBadPasswordException(string message, System.Exception innerException) : base(message, GPErrorCode.NewUserBadPasswords, innerException)
        {
        }
    }
}