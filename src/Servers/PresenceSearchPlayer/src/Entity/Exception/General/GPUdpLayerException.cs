using UniSpy.Server.PresenceSearchPlayer.Entity.Enumerate;

namespace UniSpy.Server.PresenceSearchPlayer.Entity.Exception.General
{
    public class GPUdpLayerException : GPException
    {
        public GPUdpLayerException() : base("Unkown UDP layer error!", GPErrorCode.UdpLayer)
        {
        }

        public GPUdpLayerException(string message) : base(message, GPErrorCode.UdpLayer)
        {
        }

        public GPUdpLayerException(string message, System.Exception innerException) : base(message, GPErrorCode.UdpLayer, innerException)
        {
        }
    }
}