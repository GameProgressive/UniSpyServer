using UniSpy.Server.PresenceSearchPlayer.Enumerate;

namespace UniSpy.Server.PresenceSearchPlayer.Exception.General
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