namespace PresenceConnectionManager.Interface
{
    public interface IFriendStatusUpdate
    {
        void SubscribeToStatusChange();

        void UnsubscribeToStatusChange();

        void UpdateFriendStatus();
    }
}
