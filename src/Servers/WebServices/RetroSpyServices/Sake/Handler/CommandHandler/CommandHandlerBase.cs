using WebServices.RetroSpyServices.Sake.Entity.Structure.Request;

namespace WebServices.RetroSpyServices.Sake.Handler.CommandHandler
{
    public abstract class CommandHandlerBase
    {
        protected SakeRequestBase _request;
        protected object _response;

        public CommandHandlerBase(SakeRequestBase request)
        {
            _request = request;
        }
        public virtual object Handle()
        {
            DataOperation();
            ConstructResponse();
            return _response;
        }

        public virtual void DataOperation()
        { }

        public virtual void ConstructResponse()
        { }
    }
}
