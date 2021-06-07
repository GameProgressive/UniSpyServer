using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Exception.General;

namespace PresenceSearchPlayer.Entity.Exception.AuthAdd
{
    public class GPAuthAddBadFormException : GPException
    {
        public GPAuthAddBadFormException() : base("The authentication is in bad form!", GPErrorCode.AuthAddBadForm)
        {
        }

        public GPAuthAddBadFormException(string message) : base(message, GPErrorCode.AuthAddBadForm)
        {
        }

        public GPAuthAddBadFormException(string message, System.Exception innerException) : base(message, GPErrorCode.AuthAddBadForm, innerException)
        {
        }
    }
}