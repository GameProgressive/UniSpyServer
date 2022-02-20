namespace UniSpyServer.Servers.WebServer.Abstraction.Sake
{
    public abstract class ResponseBase : Abstraction.ResponseBase
    {
        protected ResponseBase(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }
    }
}