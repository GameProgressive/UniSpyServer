using GameSpyLib.Common;
using GameSpyLib.Logging;
using PresenceSearchPlayer.Enumerator;
using PresenceSearchPlayer.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PresenceSearchPlayer
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
                    case "valid":
                        ValidHandler.IsEmailValid(session, recv);
                        break;
                    case "nicks":
                        NickHandler.SearchNicks(session, recv);
                        break;
                    case "pmatch":
                        PmatchHandler.PlayerMatch(session, recv);
                        break;
                    case "check":
                        CheckHandler.CheckProfileid(session, recv);
                        break;
                    case "newuser":
                        NewUserHandler.NewUser(session, recv);
                        break;
                    case "searchunique":
                        SearchUniqueHandler.SearchProfileWithUniquenick(session, recv);
                        break;
                    case "others":
                        OthersHandler.SearchOtherBuddy(session, recv);
                        break;
                    case "otherslist":
                        OthersListHandler.SearchOtherBuddyList(session, recv);
                        break;
                    case "uniquesearch":
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
