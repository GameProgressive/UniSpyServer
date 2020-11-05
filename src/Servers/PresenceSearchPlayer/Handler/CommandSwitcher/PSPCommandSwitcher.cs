using UniSpyLib.Logging;
using UniSpyLib.MiscMethod;
using PresenceSearchPlayer.Entity.Structure;
using PresenceSearchPlayer.Handler.CommandHandler.Check;
using PresenceSearchPlayer.Handler.CommandHandler.NewUser;
using PresenceSearchPlayer.Handler.CommandHandler.Nick;
using PresenceSearchPlayer.Handler.CommandHandler.Others;
using PresenceSearchPlayer.Handler.CommandHandler.OthersList;
//using PresenceSearchPlayer.Handler.CommandHandler.Pmatch;
using PresenceSearchPlayer.Handler.CommandHandler.Search;
using PresenceSearchPlayer.Handler.CommandHandler.SearchUnique;
using PresenceSearchPlayer.Handler.CommandHandler.UniqueSearch;
using PresenceSearchPlayer.Handler.CommandHandler.Valid;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using PresenceSearchPlayer.Network;

namespace PresenceSearchPlayer.Handler.CommandSwitcher
{
    public class PSPCommandSwitcher
    {
        public static void Switch(PSPSession session, string message)
        {

            try
            {
                if (message[0] != '\\')
                {
                    LogWriter.ToLog(LogEventLevel.Error, "Invalid request recieved!");
                    return;
                }
                string[] requests = message.Split("\\final\\", StringSplitOptions.RemoveEmptyEntries);

                foreach (string request in requests)
                {
                    if (request.Length < 1)
                    {
                        continue;
                    }

                    // Read client message, and parse it into key value pairs
                    Dictionary<string, string> recv = GameSpyUtils.ConvertRequestToKeyValue(request);

                    switch (recv.Keys.First())
                    {
                        case PSPRequestName.Search:
                            new SearchHandler(session, recv).Handle();
                            break;
                        case PSPRequestName.Valid:
                            new ValidHandler(session, recv).Handle();
                            break;
                        case PSPRequestName.Nicks:
                            new NicksHandler(session, recv).Handle();
                            break;
                        case PSPRequestName.PMatch:
                            //    PmatchHandler pmatch = new PmatchHandler(recv);
                            //    pmatch.Handle(session);
                            break;
                        case PSPRequestName.Check:
                            new CheckHandler(session, recv).Handle();
                            break;
                        case PSPRequestName.NewUser:
                            new NewUserHandler(session, recv).Handle();
                            break;
                        case PSPRequestName.SearchUnique:
                            new SearchUniqueHandler(session, recv).Handle();
                            break;
                        case PSPRequestName.Others:
                            new OthersHandler(session, recv).Handle();
                            break;
                        case PSPRequestName.OtherList:
                            new OthersListHandler(session, recv).Handle();
                            break;
                        case PSPRequestName.UniqueSearch:
                            new UniqueSearchHandler(session, recv).Handle();
                            break;
                        default:
                            LogWriter.UnknownDataRecieved(message);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                LogWriter.ToLog(e);
            }
        }
    }
}
