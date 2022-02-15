using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.UniSpyLib.Events
{
    public delegate void OnConnectedEventHandler();
    public delegate void OnReceivedEventHandler(object buffer);
    public delegate void OnDisconnectedEventHandler();
}