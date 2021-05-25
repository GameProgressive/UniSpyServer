using System;
using PresenceSearchPlayer.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Entity.Exceptions.Login
{
    public class GPLoginServerAuthFailedException : GPExceptionBase
    {
        public GPLoginServerAuthFailedException() : base("Login server authentication failed!")
        {
        }

        public GPLoginServerAuthFailedException(string message) : base(message)
        {
        }

        public GPLoginServerAuthFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}