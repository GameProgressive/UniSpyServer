using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Logging;

namespace GameSpyLib.Common.BaseClass
{
    public abstract class CommandHandlerBase
    {
        protected IClient _client;
        public CommandHandlerBase(IClient client)
        {
            _client = client;
        }

        public virtual void Handle()
        {
            LogWriter.LogCurrentClass(this);
        }
    }
}
