using Chat.Entity.Structure;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Entity.Interface
{
    public interface ICommandHandler
    {
        public object GetInstance();
        public string GetHandlerType();
        public void Handle();
    }
}
