using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Enumerate;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General
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