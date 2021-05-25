using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Entity.Exceptions.General
{
    public class GPUdpLayerException : GPExceptionBase
    {
        public GPUdpLayerException() : base("Unkown UDP layer error!")
        {
            ErrorCode = GPErrorCode.UdpLayer;
        }

        public GPUdpLayerException(string message) : base(message)
        {
            ErrorCode = GPErrorCode.UdpLayer;
        }

        public GPUdpLayerException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = GPErrorCode.UdpLayer;
        }
    }
}