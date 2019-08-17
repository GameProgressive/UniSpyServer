using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer.Handler
{
    public class NewUserHandler
    {
        /// <summary>
        /// Creates an account and use new account to login
        /// </summary>
        /// <param name="client">The client that sended the data</param>
        /// <param name="dict">The request that the stream sended</param>
        public static void NewUser(GPSPClient client, Dictionary<string, string> dict)
        {
            //Format the password for our database storage
            GPSPHandler.ProessPassword(dict);
            GPErrorCode error = GPSPHandler.IsNewUserContainAllKeys(dict);
            //if there do not recieved right <key,value> pairs we send error
            if (error != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(client.Stream, error, "Error recieving request. Please check the input!");
                return;
            }
            //Check the nick and uniquenick is formated correct and uniquenick is existed in database
            string sendingBuffer;
            error = GPSPHandler.IsEmailNickUniquenickValied(dict, GPSPHandler.DBQuery);
            if (error != GPErrorCode.NoError)
            {
                sendingBuffer = string.Format(@"\nur\{0}\final\", (int)error);
                client.Stream.SendAsync(sendingBuffer);
                return;
            }

            //we get the userid in database. If no userid found according to email we create one 
            //and store the new account into database.
            uint userid = GPSPHandler.DBQuery.GetuseridFromEmail(dict);
            //create a profile according to userid
            uint pid = GPSPHandler.DBQuery.CreateUserWithNickOrUniquenick(dict, userid);
            //Finally we send the create correct to client
            client.Stream.SendAsync(@"\nur\0\pid\{0}\final\", pid);

        }


    }
}
