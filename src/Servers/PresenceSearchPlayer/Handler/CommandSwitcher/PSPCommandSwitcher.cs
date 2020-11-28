using UniSpyLib.Logging;
using UniSpyLib.MiscMethod;
using PresenceSearchPlayer.Entity.Structure;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using PresenceSearchPlayer.Network;
using PresenceSearchPlayer.Handler.CommandHandler;

namespace PresenceSearchPlayer.Handler.CommandSwitcher
{
    public class PSPCommandSwitcher
    {
        public static void Switch(PSPSession session, string rawRequest)
        {
            var requests = PSPRequestSerializer.Serialize(session, rawRequest);

            foreach (var request in requests)
            {
                switch (request.CommandName)
                {
                    case PSPRequestName.Search:
                        new SearchHandler(session, request).Handle();
                        break;
                    case PSPRequestName.Valid:
                        new ValidHandler(session, request).Handle();
                        break;
                    case PSPRequestName.Nicks:
                        new NicksHandler(session, request).Handle();
                        break;
                    case PSPRequestName.PMatch:
                        //    PmatchHandler pmatch = new PmatchHandler(request);
                        //    pmatch.Handle(session);
                        break;
                    case PSPRequestName.Check:
                        new CheckHandler(session, request).Handle();
                        break;
                    case PSPRequestName.NewUser:
                        new NewUserHandler(session, request).Handle();
                        break;
                    case PSPRequestName.SearchUnique:
                        new SearchUniqueHandler(session, request).Handle();
                        break;
                    case PSPRequestName.Others:
                        new OthersHandler(session, request).Handle();
                        break;
                    case PSPRequestName.OtherList:
                        new OthersListHandler(session, request).Handle();
                        break;
                    case PSPRequestName.UniqueSearch:
                        new UniqueSearchHandler(session, request).Handle();
                        break;
                    default:
                        LogWriter.UnknownDataRecieved(rawRequest);
                        break;
                }
            }
        }
    }
}
