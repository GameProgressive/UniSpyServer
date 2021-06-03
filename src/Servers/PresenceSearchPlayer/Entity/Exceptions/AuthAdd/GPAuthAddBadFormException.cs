using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.AuthAdd
{
    public class GPAuthAddBadFormException : GPExceptionBase
    {
        public GPAuthAddBadFormException():base("The authentication is in bad form!")
        {
            ErrorCode = GPErrorCode.AuthAddBadForm;
        }

        public GPAuthAddBadFormException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.AuthAddBadForm;
        }

        public GPAuthAddBadFormException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.AuthAddBadForm;
        }
    }
}