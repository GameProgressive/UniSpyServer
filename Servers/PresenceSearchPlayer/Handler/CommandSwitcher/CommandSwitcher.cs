using GameSpyLib.Logging;
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
                        SearchHandler search = new SearchHandler(recv);
                        search.Handle(session);
                        break;
                    case "valid"://is email format valid
                        ValidHandler valid = new ValidHandler(recv);
                        valid.Handle(session);
                        break;
                    case "nicks":// search an user with nick name
                        NickHandler nick = new NickHandler(recv);
                        nick.Handle(session);
                        break;
                    //case "pmatch":
                    //    PmatchHandler pmatch = new PmatchHandler(recv);
                    //    pmatch.Handle(session);
                        //break;
                    case "check":
                        CheckHandler check = new CheckHandler(recv);
                        check.Handle(session);
                        break;
                    case "newuser"://create an new user
                        NewUserHandler newUser = new NewUserHandler(recv);
                        newUser.Handle(session);
                        break;
                    case "searchunique"://search an user with uniquenick
                        SearchUniqueHandler searchUnique = new SearchUniqueHandler(recv);
                        searchUnique.Handle(session);
                        break;
                    case "others"://search 
                        OthersHandler others = new OthersHandler(recv);
                        others.Handle(session);
                        break;
                    case "otherslist"://search other players friend list to see who is in his list?
                        OthersListHandler othersList = new OthersListHandler(recv);
                        othersList.Handle(session);
                        break;
                    case "uniquesearch"://search a user with uniquenick and namespaceid
                        UniqueSearchHandler uniqueSearch = new UniqueSearchHandler(recv);
                        uniqueSearch.Handle(session);
                        break;
                    default:
                        session.UnknownDataRecived(command, recv);
                        break;
                }
            }
            catch (Exception e)
            {
                LogWriter.Log.WriteException(e);
                Error.ErrorMsg.SendGPSPError(session, Enumerator.GPErrorCode.General, 0);
            }
        }
    }
}
