using GameSpyLib.Common;
using GameSpyLib.Logging;
using PresenceConnectionManager.Enumerator;
using System;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler
{
    public class NewUserHandler
    {
        /// <summary>
        /// Whenever the "newuser" command is recieved, this method is called to
        /// add the new users information into the database
        /// </summary>
        /// <param name="dict">Array of parms sent by the server</param>
        public static void CreateNewUser(GPCMClient client, Dictionary<string, string> dict)
        {
            // Make sure the user doesnt exist already
            try
            {

                // Check to see if user exists
                if (GPCMHandler.DBQuery.UserExists(dict["nick"]))
                {
                    client.Stream.SendAsync(@"\error\\err\516\fatal\\errmsg\This account name is already in use!\id\1\final\");
                    client.Disconnect(DisconnectReason.CreateFailedUsernameExists);
                    return;
                }

                // We need to decode the Gamespy specific encoding for the password
                string Password = GamespyUtils.DecodePassword(dict["passwordenc"]);
                string Cc = (client.RemoteEndPoint.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    //? GeoIP.GetCountryCode(RemoteEndPoint.Address)
                    //: "US";
                    ? "US" : "US";

                // Attempt to create account. If Pid is 0, then we couldnt create the account. TODO: Handle Unique Nickname
                if ((client.PlayerInfo.PlayerId = GPCMHandler.DBQuery.CreateUser(dict["nick"], Password, dict["email"], Cc, dict["nick"])) == 0)
                {
                    GamespyUtils.SendGPError(client.Stream, GPErrorCode.NewUserUniquenickInUse, "An error oncurred while creating the account!");
                    client.Disconnect(DisconnectReason.CreateFailedDatabaseError);
                    return;
                }

                client.Stream.SendAsync(@"\nur\\userid\{0}\profileid\{0}\id\1\final\", client.PlayerInfo.PlayerId);
            }
            catch (Exception e)
            {
                // Check for invalid query params
                if (e is KeyNotFoundException)
                {
                    GamespyUtils.SendGPError(client.Stream, GPErrorCode.NewUserUniquenickInUse, "Invalid response received from the client!");
                }
                else
                {
                    GamespyUtils.SendGPError(client.Stream, GPErrorCode.NewUserUniquenickInUse, "An error oncurred while creating the account!");
                    LogWriter.Log.Write("An error occured while trying to create a new User account :: " + e.Message, LogLevel.Error);
                }

                client.Disconnect(DisconnectReason.GeneralError);
                return;
            }
        }

    }
}
