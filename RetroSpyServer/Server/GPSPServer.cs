using System;
using System.Collections.Generic;
using GameSpyLib;
using GameSpyLib.Database;
using GameSpyLib.Server;
using GameSpyLib.Network;
using GameSpyLib.Logging;

namespace RetroSpyServer
{
    public class GPSPServer : PresenceServer
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="databaseDriver">
        /// A connection to a database
        /// NOTE: The connection must not be null
        /// </param>
        public GPSPServer(DatabaseDriver databaseDriver) : base(databaseDriver)
        {
        }

        /*enum RequestsCheck
        {
            valid,
            nicks,
            check,
            search,
            others,
            otherslist,
            uniquesearch,
            profilelist,
            pmatch,
            newuser
        };*/

        /// <summary>
        /// This function is fired when an exception occour in the server
        /// </summary>
        /// <param name="e">The exception to be thrown</param>
        protected override void OnException(Exception e)
        {
            throw e;
        }

        /// <summary>
        /// This function is fired when a client is being accepted
        /// </summary>
        /// <param name="Stream">The stream of the client to be accepted</param>
        protected override void ProcessAccept(TCPStream Stream)
        {
            Stream.DataReceived += ProcessDataReceived;
            Stream.OnDisconnect += (stream) => stream.Dispose();
            Stream.BeginReceive();
        }

        /// <summary>
        /// This function is fired when data is received from a stream
        /// </summary>
        /// <param name="stream">The stream that sended the data</param>
        /// <param name="message">The message the stream sended</param>
        protected void ProcessDataReceived(TCPStream stream, string message)
        {
            string[] received = message.TrimStart('\\').Split('\\');
            Dictionary<string, string> dict = ConvertToKeyValue(received);
            
            switch (received[0])
            {
                case "valid":
                    IsEmailValid(stream, dict);
                    break;
                case "nicks":
                    RetriveNicknames(stream, dict);
                    break;
                case "check":
                    CheckAccount(stream, dict);
                    break;
                case "search":
                    SearchUser(stream, dict);
                    break;
                case "others":
                    ReverseBuddies(stream, dict);
                    break;
                case "otherslist":
                    OnOthersList(stream, dict);
                    break;
                case "uniquesearch":
                    SuggestUniqueNickname(stream, dict);
                    break;
                case "profilelist":
                    OnProfileList(stream, dict);
                    break;
                case "pmatch":
                    MatchProduct(stream, dict);
                    break;
                case "newuser":
                    CreateUser(stream, dict);
                    break;
                default:
                    LogWriter.Log.Write("Received unknown request " + received[0], LogLevel.Debug);
                    SendErrorAndFreeStream(stream, 0, "An invalid request was sended.");
                    break;
            }
        }
        //Enum.TryParse(received[0], out RequestsCheck request);
        //    switch (request)
        //    {
        //        case RequestsCheck.valid:
        //             IsEmailValid(stream, dict);
        //             break;
        //        case RequestsCheck.nicks:
        //             RetriveNicknames(stream, dict);
        //             break;
        //        case RequestsCheck.check:
        //             CheckAccount(stream, dict);
        //             break;
        //        case RequestsCheck.search:
        //             SearchUser(stream, dict);
        //             break;
        //        case RequestsCheck.others:
        //             ReverseBuddies(stream, dict);
        //             break;
        //        case RequestsCheck.otherslist:
        //             OnOthersList(stream, dict);
        //             break;
        //        case RequestsCheck.uniquesearch:
        //             SuggestUniqueNickname(stream, dict);
        //             break;
        //        case RequestsCheck.profilelist:
        //             OnProfileList(stream, dict);
        //             break;
        //        case RequestsCheck.pmatch:
        //             MatchProduct(stream, dict);
        //             break;
        //        case RequestsCheck.newuser:
        //             CreateUser(stream, dict);
        //             break;
        //        default:
        //            Logger.Debug("Received unknown request " + received[0]);
        //            SendError(stream, 0, "An invalid request was sended.");
        //            stream.Close(false);
        //            break;
        //    }
        //}

        private void SuggestUniqueNickname(TCPStream stream, Dictionary<string, string> dict)
        {
            PrintReceivedDictToLogger("uniquesearch", dict);
            SendErrorAndFreeStream(stream, 0, "This request is not supported yet.");
        }

        private void OnProfileList(TCPStream stream, Dictionary<string, string> dict)
        {
            PrintReceivedDictToLogger("profilelist", dict);
            SendErrorAndFreeStream(stream, 0, "This request is not supported yet.");
        }

        private void MatchProduct(TCPStream stream, Dictionary<string, string> dict)
        {
            PrintReceivedDictToLogger("pmatch", dict);
            SendErrorAndFreeStream(stream, 0, "This request is not supported yet.");
        }

        /// <summary>
        /// Creates an account
        /// </summary>
        /// <param name="stream">The stream that sended the data</param>
        /// <param name="dict">The request that the stream sended</param>
        private void CreateUser(TCPStream stream, Dictionary<string, string> dict)
        {
            PrintReceivedDictToLogger("newuser", dict);
            SendErrorAndFreeStream(stream, 0, "This request is not supported yet.");
        }

        private void OnOthersList(TCPStream stream, Dictionary<string, string> dict)
        {
            PrintReceivedDictToLogger("otherslist", dict);
            SendErrorAndFreeStream(stream, 0, "This request is not supported yet.");
        }

        private void ReverseBuddies(TCPStream stream, Dictionary<string, string> dict)
        {
            PrintReceivedDictToLogger("others", dict);
            SendErrorAndFreeStream(stream, 0, "This request is not supported yet.");
        }

        private void SearchUser(TCPStream stream, Dictionary<string, string> dict)
        {
            PrintReceivedDictToLogger("search", dict);
            SendErrorAndFreeStream(stream, 0, "This request is not supported yet.");
        }

        private void CheckAccount(TCPStream stream, Dictionary<string, string> dict)
        {
            PrintReceivedDictToLogger("check", dict);
            SendErrorAndFreeStream(stream, 0, "This request is not supported yet.");
        }

        // kou finish this !
        private void RetriveNicknames(TCPStream stream, Dictionary<string, string> dict)
        {
            PrintReceivedDictToLogger("nicks", dict);
            SendErrorAndFreeStream(stream, 0, "This request is not supported yet.");

/* Legacy C++ code to reimpliment 
bool PSServer::OnSendNicks(mdk_socket stream, const char *buf, int)
{
	std::string email = "", pass = "", gamename = "", str = "";
	bool bSendUnique = false;
	size_t i = 0;
	CResultSet *result = NULL;

	// Get data from buffer

	if (!get_gs_data(buf, email, "email"))
		return false;

	if (get_gs_data(buf, pass, "passenc"))
	{
		// Uncrypt the password
		gs_pass_decode(pass);
	}
	else
	{
		if (!get_gs_data(buf, pass, "pass"))
			return false;
	}

	if (get_gs_data(buf, gamename, "gamename"))
		bSendUnique = true;

	// Create the query and execute it
	str = "SELECT profiles.nick, profiles.uniquenick FROM profiles INNER "
		"JOIN users ON profiles.userid=users.userid WHERE users.email='";
	if (!mdk_escape_query_string(m_lpDatabase, email))
		return false;

	str += email;
	str += "' AND password='";
	if (!mdk_escape_query_string(m_lpDatabase, pass))
		return false;

	str += pass;
	str += "'";

	result = new CResultSet();

	if (!result->ExecuteQuery(m_lpDatabase, str))
	{
		delete result;
		
		WriteTCP(stream, "\\nr\\\\ndone\\final\\");
		return false;
	}
	
	if (!result->GotoFirstRow())
	{
		delete result;
		
		WriteTCP(stream, "\\nr\\\\ndone\\final\\");
		return false;
		
	}

	str = "\\nr\\" + std::to_string(result->GetTotalRows());

	// Get all the nicks and store them
	do
	{
		str += "\\nick\\";
		str += result->GetStringFromRow(0);

		if (bSendUnique)
		{
			str += "\\uniquenick\\";
			str += result->GetStringFromRow(1);
		}
	} while(result->GotoNextRow());

	str += "\\ndone\\final\\";

	// Send to the socket
	WriteTCP(stream, str);

	delete result;

	return true;
}*/

        }

        /// <summary>
        /// Checks if a provided email is valid
        /// </summary>
        /// <param name="stream">The stream that sended the data</param>
        /// <param name="dict">The request that the stream sended</param>
        private void IsEmailValid(TCPStream stream, Dictionary<string, string> dict)
        {
            if (!dict.ContainsKey("email"))
            {
                SendErrorAndFreeStream(stream, 1, "There was an error parsing an incoming request.");
                return;
            }

            try
            {
                if (databaseDriver.Query("SELECT userid FROM users WHERE LOWER(email)=@P0", dict["email"].ToLowerInvariant()).Count != 0)
                    stream.SendAsync(@"\vr\1\final\");
                else
                    stream.SendAsync(@"\vr\0\final\");
            }
            catch (Exception ex)
            {
                LogWriter.Log.Write(ex.Message, LogLevel.Error);
                SendErrorAndFreeStream(stream, 4, "This request cannot be processed because of a database error.");
            }
        }
    }
}
