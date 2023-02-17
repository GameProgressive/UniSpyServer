namespace UniSpy.Server.Core.Events
{
    public delegate void OnConnectedEventHandler();
    public delegate void OnReceivedEventHandler(object buffer);
    public delegate void OnDisconnectedEventHandler();
}