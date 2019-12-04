using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;
//last one we search with email this may get few profile so we can not return GPErrorCode
//SearchWithEmail(client,dict );
//\search\\sesskey\0\profileid\0\namespaceid\1\partnerid\0\nick\mycrysis\uniquenick\xiaojiuwo\email\koujiangheng@live.cn\gamename\gmtest\final\
//\bsrdone\more\<more>\final\
//string sendingbuffer = 
//"\\bsr\\1\\nick\\mycrysis\\uniquenick\\1\\namespaceid\\0\\firstname\\jiangheng\\lastname\\kou\\email\\koujiangheng@live.cn\\bsrdone\\0\\final\\";
//client.Stream.SendAsync(sendingbuffer);
//\more\<number of items>\final\
//\search\sesskey\0\profileid\0\namespaceid\0\nick\gbr359_jordips\gamename\gbrome\final\

namespace PresenceSearchPlayer.Handler.Search
{
    public class SearchHandler
    {
        private static List<Dictionary<string, object>> _queryResult;
        private static string _sendingBuffer;
        private static GPErrorCode _errorCode;
        private static string _errorMsg;
        private static Dictionary<string, string> _recv;

        public static void SearchUsers(GPSPSession session, Dictionary<string, string> recv)
        {
            //initialize all staitc member
            _recv = recv;
            _sendingBuffer = "";
            _errorCode = GPErrorCode.NoError;
            _errorMsg = "";
            _queryResult = null;

            IsContainAllKey();
            if (_errorCode != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, _errorCode, _errorMsg);
                return;
            }

            //we only need uniquenick to search a profile
            if (recv.ContainsKey("uniquenick"))
            {
                SearchWithUniquenick();
            }
            else if (recv.ContainsKey("nick") && recv.ContainsKey("email"))
            {
                SearchWithNickAndEmail();
            }
            else if (recv.ContainsKey("nick"))
            {
                SearchWithNick();
            }
            else if (recv.ContainsKey("email"))
            {
                SearchWithEmail();
            }
            else
            {
                session.ToLog("Unknow search request received!");
                return;
            }

            CheckDatabaseResult();
            if (_errorCode != GPErrorCode.NoError)
            {
                session.SendAsync(@"\bsrdone\\final\");
                return;
            }

            SendReponse(session);
        }


        private static void IsContainAllKey()
        {
            if (!_recv.ContainsKey("sesskey") && !_recv.ContainsKey("email") && !_recv.ContainsKey("uniquenick"))
            {
                _errorCode = GPErrorCode.Parse;
                _errorMsg = "Parsing error";
            }
            if (!_recv.ContainsKey("namespaceid"))
            {
                _recv.Add("namespaceid", "0");
            }
        }

        /// <summary>
        /// search with uniquenick
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="sendingBuffer"></param>
        /// <returns></returns>
        private static void SearchWithUniquenick()
        {
            _queryResult
                = SearchQuery.GetProfileFromUniquenick(_recv["uniquenick"], Convert.ToUInt32(_recv["namespaceid"]));
        }

        /// <summary>
        /// search with nick and email
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="sendingBuffer"></param>
        /// <returns></returns>
        private static void SearchWithNickAndEmail()
        {
            _queryResult = SearchQuery.GetProfileFromNickEmail(_recv["nick"],_recv["email"],Convert.ToUInt16(_recv["namespaceid"]));
        }

        private static void SearchWithNick()
        {
            _queryResult = SearchQuery.GetProfileFromNick(_recv["nick"], Convert.ToUInt16(_recv["namespaceid"]));
        }

        /// <summary>
        /// only search with email, so we may get few profiles for one userid
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="sendingBuffer"></param>
        /// <returns></returns>
        private static void SearchWithEmail()
        {
            _queryResult = SearchQuery.GetProfileFromEmail(_recv);
        }

        private static void CheckDatabaseResult()
        {
            if (_queryResult == null)
            {
                _errorCode = GPErrorCode.Search;
                _errorMsg = "No match found!";
            }
        }

        private static void SendReponse(GPSPSession session)
        {
            int index;
            if (_queryResult.Count == 1)
            {
                index = 0;
            }
            //We received request which needs more results.
            else
            {
                if (_recv.ContainsKey("skip"))
                {
                    index = Convert.ToInt16(_recv["skip"]);
                }
                else
                {
                    index = 0;
                }
            }
            if (index < _queryResult.Count)
            {
                _sendingBuffer = @"\bsr\" + Convert.ToUInt16(_queryResult[index]["profileid"]);
                _sendingBuffer += @"\nick\" + _queryResult[index]["nick"];
                _sendingBuffer += @"\uniquenick\" + _queryResult[index]["uniquenick"];
                _sendingBuffer += @"\namespaceid\" + Convert.ToUInt16(_queryResult[index]["namespaceid"]);
                _sendingBuffer += @"\firstname\" + _queryResult[index]["firstname"];
                _sendingBuffer += @"\lastname\" + _queryResult[index]["lastname"];
                _sendingBuffer += @"\email\" + _queryResult[index]["email"];
                _sendingBuffer += @"\bsrdone\\more\" + (_queryResult.Count - 1) + @"\final\";
                session.SendAsync(_sendingBuffer);
            }
        }
    }
}


