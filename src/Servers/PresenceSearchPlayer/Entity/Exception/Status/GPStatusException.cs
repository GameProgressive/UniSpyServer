using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Exception.General;

namespace PresenceSearchPlayer.Entity.Exception.Status
{
    public class GPStatusException : GPException
    {
        public GPStatusException() : base("Unknown error happen when processing player status", GPErrorCode.Status)
        {
        }

        public GPStatusException(string message) : base(message, GPErrorCode.Status)
        {
        }

        public GPStatusException(string message, System.Exception innerException) : base(message, GPErrorCode.Status, innerException)
        {
        }
    }
}