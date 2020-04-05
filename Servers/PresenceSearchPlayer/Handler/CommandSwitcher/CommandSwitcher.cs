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
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler.CommandHandler
{
    public class CommandSwitcher
    {
        public static void Switch(GPSPSession session, Dictionary<string, string> recv)
        {
            string command = recv.Keys.First();

            try
            {
                switch (command)
                {
                    case "search":
                        new SearchHandler().Handle(session, recv);
                        break;

                    case "valid"://is email format valid
                        new ValidHandler().Handle(session, recv);
                        break;

                    case "nicks":// search an user with nick name
                        new NickHandler().Handle(session, recv);
                        break;

                    //case "pmatch":
                    //    PmatchHandler pmatch = new PmatchHandler(recv);
                    //    pmatch.Handle(session);
                    //    break;

                    case "check":
                        new CheckHandler().Handle(session, recv);
                        break;

                    case "newuser"://create an new user
                        new NewUserHandler().Handle(session, recv);
                        break;

                    case "searchunique"://search an user with uniquenick
                        new SearchUniqueHandler().Handle(session, recv);
                        break;

                    case "others"://search 
                        new OthersHandler().Handle(session, recv);
                        break;

                    case "otherslist"://search other players friend list to see who is in his list?
                        new OthersListHandler().Handle(session, recv);
                        break;

                    case "uniquesearch"://search a user with uniquenick and namespaceid
                        new UniqueSearchHandler().Handle(session, recv);
                        break;

                    default:
                        session.UnknownDataReceived(recv);
                        break;
                }
            }
            catch
            {
                session.UnknownDataReceived(recv);
            }
        }
    }
}
