using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace CDKey.Handler.CmdSwitcher
{
    internal sealed class CDKeyCmdHandlerFactory : UniSpyCmdHandlerFactory
    {
        public CDKeyCmdHandlerFactory(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        public override IUniSpyHandler Deserialize()
        {
            switch (_request.CommandName)
            {
                //keep client alive request, we skip this
                case "ka":
                    throw new NotImplementedException();
                case "auth":
                    throw new NotImplementedException();
                case "resp":
                    throw new NotImplementedException();
                case "skey":
                    throw new NotImplementedException();
                case "disc"://disconnect from server
                    throw new NotImplementedException();
                case "uon":
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
