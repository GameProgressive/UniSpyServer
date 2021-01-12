using System;
using CDKey.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace CDKey.Handler.CmdSwitcher
{
    internal class CDKeyCommandHandlerSerializer : UniSpyCmdHandlerSerializerBase
    {
        public CDKeyCommandHandlerSerializer(IUniSpySession session,IUniSpyRequest request) : base(session,request)
        {
        }

        public override IUniSpyHandler Serialize()
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
                    LogWriter.UnknownDataRecieved((string)_request.RawRequest);
                    return null;
            }
        }
    }
}
