namespace Chat.Entity.Interface
{
    public interface ICommandHandler
    {
        public object GetInstance();
        public string GetHandlerType();
        public void Handle();
    }
}
