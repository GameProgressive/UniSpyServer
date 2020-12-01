using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class CommandHandlerSerializerBase
    {
        protected IRequest _request;
        protected ISession _session;
        public CommandHandlerSerializerBase(ISession session,IRequest request)
        {
            _request = request;
            _session = session;
            LogWriter.LogCurrentClass(this);
        }

        public abstract IHandler Serialize();
    }
}
