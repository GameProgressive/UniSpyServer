using RetroSpyServer.Servers.GPSP;
using RetroSpyServer.Servers.GPCM;

namespace RetroSpyServer.Application
{
    public delegate void ConnectionUpdate(GPCMClient client);

    public delegate void GpspConnectionClosed(GPSPClient client);

    public delegate void GpcmConnectionClosed(GPCMClient client);

    public delegate void GpcmStatusChanged(GPCMClient client);
}
