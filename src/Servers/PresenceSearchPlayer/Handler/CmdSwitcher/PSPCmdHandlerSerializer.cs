using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure;
using PresenceSearchPlayer.Handler.CmdHandler;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace PresenceSearchPlayer.Handler.CmdSwitcher
{
    public class PSPCmdHandlerSerializer : UniSpyCmdHandlerSerializerBase
    {
        protected new PSPRequestBase _request => (PSPRequestBase)base._request;
        public PSPCmdHandlerSerializer(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        public override IUniSpyHandler Serialize()
        {
            switch (_request.CommandName)
            {
                case PSPRequestName.Search:
                    return new SearchHandler(_session, _request);
                case PSPRequestName.Valid:
                    return new ValidHandler(_session, _request);
                case PSPRequestName.Nicks:
                    return new NicksHandler(_session, _request);
                case PSPRequestName.PMatch:
                    //    PmatchHandler pmatch = new PmatchHandler(request);
                    //    pmatch.Handle(session);
                    throw new NotImplementedException();
                case PSPRequestName.Check:
                    return new CheckHandler(_session, _request);
                case PSPRequestName.NewUser:
                    return new NewUserHandler(_session, _request);
                case PSPRequestName.SearchUnique:
                    return new SearchUniqueHandler(_session, _request);
                case PSPRequestName.Others:
                    return new OthersHandler(_session, _request);
                case PSPRequestName.OtherList:
                    return new OthersListHandler(_session, _request);
                case PSPRequestName.UniqueSearch:
                    return new UniqueSearchHandler(_session, _request);
                default:
                    LogWriter.UnknownDataRecieved(_request.RawRequest);
                    return null;

            }
        }
    }
}

