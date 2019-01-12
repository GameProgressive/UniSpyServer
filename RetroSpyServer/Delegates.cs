using RetroSpyServer.Server;

namespace RetroSpyServer
{
    public delegate void ConnectionUpdate(GPCMClient client);

    public delegate void GpspConnectionClosed(GPCMClient client);

    public delegate void GpcmConnectionClosed(GPCMClient client);
}
