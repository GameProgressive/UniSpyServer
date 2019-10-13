using GameSpyLib.Common;
using GameSpyLib.Logging;
using PresenceSearchPlayer.DatabaseQuery;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;
using System.Text;

/////////////////////////Finished/////////////////////////////////
namespace PresenceSearchPlayer.Handler
{

    /// <summary>
    /// Uses a nick to find how many uniquenick is in this nick
    /// </summary>
    public class NickHandler
    {
        static List<Dictionary<string, object>> _queryResult;
        static Dictionary<string, string> _recv;
        /// <summary>
        /// Get nickname through email and password
        /// </summary>
        /// <param name="session"></param>
        /// <param name="_recv"></param>
        public static void SearchNicks(GPSPSession session,Dictionary<string,string> recv)
        {
            _recv = recv;
            //Format the password for our database storage
            //if not recieved correct request we terminate
            GPErrorCode error = IsSearchNicksContianAllKeys();
            if (error != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, (int)error, "Error recieving SearchNicks request.");
                return;
            }

            if (!_recv.ContainsKey("namespaceid"))
            {
                _recv.Add("namespaceid", "0");
            }
            try
            {
                //get nicknames from GPSPDBQuery class
                _queryResult = NickQuery.RetriveNicknames(_recv);
            }
            catch (Exception ex)
            {
                LogWriter.Log.Write(ex.Message, LogLevel.Error);
                GameSpyUtils.SendGPError(session, GPErrorCode.DatabaseError, "This request cannot be processed because of a database error.");
                return;
            }

            if (_queryResult.Count < 1)
            {
                GameSpyUtils.SendGPError(session, GPErrorCode.DatabaseError, "No match found !");
                return;
            }

            string sendingBuffer;
            sendingBuffer = @"\nr\";
            foreach (Dictionary<string, object> row in _queryResult)
            {
                sendingBuffer += @"\nick\";
                sendingBuffer += row["nick"];
                sendingBuffer += @"\uniquenick\";
                sendingBuffer += row["uniquenick"];

            }

            sendingBuffer += @"\ndone\final\";
            session.SendAsync(sendingBuffer);
        }

        public static GPErrorCode IsSearchNicksContianAllKeys()
        {
            if (!_recv.ContainsKey("email"))
            {

                return GPErrorCode.Parse;
            }

            // First, we try to receive an encoded password
            if (!_recv.ContainsKey("passenc"))
            {
                // If the encoded password is not sended, we try receiving the password in plain text
                if (!_recv.ContainsKey("pass"))
                {
                    // No password is specified, we cannot continue                   
                    return GPErrorCode.Parse;
                }
            }
            return GPErrorCode.NoError;
        }
    }
}
