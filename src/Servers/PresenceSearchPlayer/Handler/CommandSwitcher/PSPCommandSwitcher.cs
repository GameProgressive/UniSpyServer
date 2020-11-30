using UniSpyLib.Logging;
using PresenceSearchPlayer.Entity.Structure;
using PresenceSearchPlayer.Handler.CommandHandler;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace PresenceSearchPlayer.Handler.CommandSwitcher
{
    public class PSPCommandSwitcher : CommandSerializerBase
    {
        protected new string _rawRequest;
        public PSPCommandSwitcher(ISession session, object rawRequest) : base(session, rawRequest)
        {
        }

        public override void Serialize()
        {
            var requests = PSPRequestSerializer.Serialize(_session, _rawRequest);

            foreach (var request in requests)
            {
                switch (request.CommandName)
                {
                    case PSPRequestName.Search:
                        new SearchHandler(_session, request).Handle();
                        break;
                    case PSPRequestName.Valid:
                        new ValidHandler(_session, request).Handle();
                        break;
                    case PSPRequestName.Nicks:
                        new NicksHandler(_session, request).Handle();
                        break;
                    case PSPRequestName.PMatch:
                        //    PmatchHandler pmatch = new PmatchHandler(request);
                        //    pmatch.Handle(session);
                        break;
                    case PSPRequestName.Check:
                        new CheckHandler(_session, request).Handle();
                        break;
                    case PSPRequestName.NewUser:
                        new NewUserHandler(_session, request).Handle();
                        break;
                    case PSPRequestName.SearchUnique:
                        new SearchUniqueHandler(_session, request).Handle();
                        break;
                    case PSPRequestName.Others:
                        new OthersHandler(_session, request).Handle();
                        break;
                    case PSPRequestName.OtherList:
                        new OthersListHandler(_session, request).Handle();
                        break;
                    case PSPRequestName.UniqueSearch:
                        new UniqueSearchHandler(_session, request).Handle();
                        break;
                    default:
                        LogWriter.UnknownDataRecieved(_rawRequest);
                        break;
                }
            }
        }
    }
}
