using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Exception.General;

namespace PresenceSearchPlayer.Entity.Exception.AuthAdd
{
    public class GPAuthAddException : GPException
    {
        public GPAuthAddException() : base("The adding of authentication failed!", GPErrorCode.AuthAdd)
        {
        }

        public GPAuthAddException(string message) : base(message, GPErrorCode.AuthAdd)
        {
        }

        public GPAuthAddException(string message, System.Exception innerException) : base(message, GPErrorCode.AuthAdd, innerException)
        {
        }
    }
}