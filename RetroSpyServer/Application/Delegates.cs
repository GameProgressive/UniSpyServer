using RetroSpyServer.Servers.GPSP;
using RetroSpyServer.Servers.GPCM;
using RetroSpyServer.Servers.ServerBrowser;
using RetroSpyServer.Servers.PeerChat;

namespace RetroSpyServer.Application
{
    public delegate void ConnectionUpdate(GPCMClient client);

    public delegate void GpspConnectionClosed(GPSPClient client);

    public delegate void GpcmConnectionClosed(GPCMClient client);

    public delegate void GpcmStatusChanged(GPCMClient client);

    public delegate void SBConnectionClosed(SBClient client);

    public delegate void PeerChatConnectionClosed(PeerChatClient client);
}
