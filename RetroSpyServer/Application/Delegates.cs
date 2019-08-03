using RetroSpyServer.Servers.GPSP;
using RetroSpyServer.Servers.GPCM;
using RetroSpyServer.Servers.ServerBrowser;
using RetroSpyServer.Servers.PeerChat;

namespace RetroSpyServer.Application
{
    public delegate void GPCMConnectionUpdate(GPCMClient client);
    public delegate void GPCMConnectionClosed(GPCMClient client);
    public delegate void GPCMStatusChanged(GPCMClient client);

    public delegate void GPSPConnectionClosed(GPSPClient client);

    public delegate void ServerBrowserConnectionClosed(SBClient client);

    public delegate void ChatConnectionClosed(ChatClient client);
}
