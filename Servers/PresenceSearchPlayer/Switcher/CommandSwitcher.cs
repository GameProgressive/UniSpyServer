using GameSpyLib.Logging;
using PresenceSearchPlayer.Handler.Check;
using PresenceSearchPlayer.Handler.NewUser;
using PresenceSearchPlayer.Handler.Nick;
using PresenceSearchPlayer.Handler.Others;
using PresenceSearchPlayer.Handler.OthersList;
using PresenceSearchPlayer.Handler.Pmatch;
using PresenceSearchPlayer.Handler.Search;
using PresenceSearchPlayer.Handler.SearchUnique;
using PresenceSearchPlayer.Handler.UniqueSearch;
using PresenceSearchPlayer.Handler.Valid;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler
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
                        SearchHandler.SearchUsers(session, recv);
                        break;
                    case "valid"://is email format valid
                        ValidHandler valid = new ValidHandler(recv);
                        valid.Handle(session);
                        break;
                    case "nicks":// search an user with nick name
                        NickHandler nick = new NickHandler(recv);
                        nick.Handle(session);
                        break;
                    case "pmatch":
                        PmatchHandler.PlayerMatch(session, recv);
                        break;
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
                        OthersListHandler.SearchOtherBuddyList(session, recv);
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
