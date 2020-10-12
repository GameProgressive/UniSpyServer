namespace Chat.Entity.Interface
{
    public interface ICommandHandler
    {
         object GetInstance();
         string GetHandlerType();
         void Handle();
    }
}
