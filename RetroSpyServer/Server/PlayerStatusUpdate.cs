namespace RetroSpyServer.Server
{
    public class PlayerStatusUpdate
    {
        public GPCMClient Client { get; protected set; }

        public LoginStatus Status { get; protected set; }

        public PlayerStatusUpdate(GPCMClient client, LoginStatus status)
        {
            Client = client;
            Status = status;
        }
    }
}
