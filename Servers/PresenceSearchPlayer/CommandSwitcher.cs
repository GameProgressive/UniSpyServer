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
        public static void Switch(GPSPClient client,Dictionary<string,string> recv)
        {
            string command = recv.Keys.First();
            try
            {

                switch (command)
                {
                    case "search":
                        SearchHandler.SearchUsers(client, recv);
                        break;
                    case "valid":
                        ValidHandler.IsEmailValid(client, recv);
                        break;
                    case "nicks":
                        NickHandler.SearchNicks(client, recv);
                        break;
                    case "pmatch":
                        PmatchHandler.PlayerMatch(client, recv);
                        break;
                    case "check":
                        CheckHandler.CheckProfileid(client, recv);
                        break;
                    case "newuser":
                        NewUserHandler.NewUser(client, recv);
                        break;
                    case "searchunique":
                        SearchUniqueHandler.SearchProfileWithUniquenick(client, recv);
                        break;
                    case "others":
                        OthersHandler.SearchOtherBuddy(client, recv);
                        break;
                    case "otherslist":
                        OthersListHandler.SearchOtherBuddyList(client, recv);
                        break;
                    case "uniquesearch":
                        UniqueSearchHandler.SuggestUniqueNickname(client, recv);
                        break;
                    default:
                        LogWriter.Log.Write("[GPSP] received unknown data " + command, LogLevel.Debug);
                        GameSpyUtils.PrintReceivedGPDictToLogger(command, recv);
                        GameSpyUtils.SendGPError(client, GPErrorCode.Parse, "An invalid request was sended.");
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
