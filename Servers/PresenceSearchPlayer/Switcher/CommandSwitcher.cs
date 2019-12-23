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
                        ValidHandler.IsEmailValid(session, recv);
                        break;
                    case "nicks":// search an user with nick name
                        NickHandler.SearchNicks(session, recv);
                        break;
                    case "pmatch":
                        PmatchHandler.PlayerMatch(session, recv);
                        break;
                    case "check":
                        CheckHandler check = new CheckHandler(recv);
                        check.Handle(session);
                        break;
                    case "newuser"://create an new user
                        NewUserHandler.NewUser(session, recv);
                        break;
                    case "searchunique"://search an user with uniquenick
                        SearchUniqueHandler.SearchProfileWithUniquenick(session, recv);
                        break;
                    case "others"://search 
                        OthersHandler.SearchOtherBuddy(session, recv);
                        break;
                    case "otherslist"://search other players friend list to see who is in his list?
                        OthersListHandler.SearchOtherBuddyList(session, recv);
                        break;
                    case "uniquesearch"://search a user with uniquenick and namespaceid
                        UniqueSearchHandler.SuggestUniqueNickname(session, recv);
                        break;
                    default:
                        session.UnknownDataRecived(command, recv);
                        break;
                }
            }
            catch (Exception e)
            {
                LogWriter.Log.WriteException(e);
            }
        }
    }
}
