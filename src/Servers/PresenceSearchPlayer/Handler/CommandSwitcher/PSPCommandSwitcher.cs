using GameSpyLib.Logging;
using GameSpyLib.MiscMethod;
using PresenceSearchPlayer.Entity.Enumerator;
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
using System;
using System.Collections.Generic;
using System.Linq;

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
                        case PSPRequestName.SearchAccount:
                            new SearchHandler(session, recv).Handle();
                            break;
                        case PSPRequestName.EmailValidCheck:
                            new ValidHandler(session, recv).Handle();
                            break;
                        case PSPRequestName.SearchByNickName:
                            new NicksHandler(session, recv).Handle();
                            break;
                        case PSPRequestName.PlayerSearch:
                            //    PmatchHandler pmatch = new PmatchHandler(recv);
                            //    pmatch.Handle(session);
                            break;
                        case PSPRequestName.CheckAccountValidation:
                            new CheckHandler(session, recv).Handle();
                            break;
                        case PSPRequestName.CreateNewUser:
                            new NewUserHandler(session, recv).Handle();
                            break;
                        case PSPRequestName.SearchUserByUniqueNickName:
                            new SearchUniqueHandler(session, recv).Handle();
                            break;
                        case PSPRequestName.SearchUsersInformation:
                            new OthersHandler(session, recv).Handle();
                            break;
                        case PSPRequestName.SearchFriendList:
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
