using GameSpyLib.Common.BaseClass;
using GameSpyLib.Logging;
using GameSpyLib.MiscMethod;
using PresenceSearchPlayer.Entity.Enumerator;
using PresenceSearchPlayer.Entity.Structure.Model;
using PresenceSearchPlayer.Enumerator;
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
using System;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler.CommandSwitcher
{
    public class PSPCommandSwitcher : CommandSwitcherBase
    {
        public void Switch(PSPSession session, string message)
        {

            try
            {
                if (message[0] != '\\')
                {
                    GameSpyUtils.SendGPError(session, GPError.Parse, "An invalid request was sended.");
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
                        case "search":
                            new SearchHandler(session, recv).Handle();
                            break;

                        case "valid"://is email format valid
                            new ValidHandler(session, recv).Handle();
                            break;

                        case "nicks":// search an user with nick name
                            new NickHandler(session, recv).Handle();
                            break;

                        //case "pmatch":
                        //    PmatchHandler pmatch = new PmatchHandler(recv);
                        //    pmatch.Handle(session);
                        //    break;

                        case "check":
                            new CheckHandler(session, recv).Handle();
                            break;

                        case "newuser"://create an new user
                            new NewUserHandler(session, recv).Handle();
                            break;

                        case "searchunique"://search an user with uniquenick
                            new SearchUniqueHandler(session, recv).Handle();
                            break;

                        case "others"://search 
                            new OthersHandler(session, recv).Handle();
                            break;

                        case "otherslist"://search other players friend list to see who is in his list?
                            new OthersListHandler(session, recv).Handle();
                            break;

                        case "uniquesearch"://search a user with uniquenick and namespaceid
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
