using GameSpyLib.Common;
using PresenceSearchPlayer.DatabaseQuery;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer.Handler
{
    public class SearchHandler
    {
        public static void SearchUsers(GPSPClient client, Dictionary<string, string> dict)
        {
            //string sendingbuffer = "\\bsr\\1\\nick\\mycrysis\\uniquenick\\1\\namespaceid\\0\\firstname\\jiangheng\\lastname\\kou\\email\\koujiangheng@live.cn\\bsrdone\\0\\final\\";
            //client.Stream.SendAsync(sendingbuffer);

            string sendingBuffer;
            GPErrorCode error;
            error = IsLogIn(dict);
            //if (error != GPErrorCode.NoError)
            //{
            //    GameSpyUtils.SendGPError(client.Stream, error, "You must login to use search functions");
            //    return;
            //}

            //we only need uniquenick to search a profile
            if (dict.ContainsKey("uniquenick"))
            {
                error = SearchWithUniquenick(dict, out sendingBuffer);
                if (error != GPErrorCode.NoError)
                {
                    GameSpyUtils.SendGPError(client.Stream, error, sendingBuffer);
                }
                else
                {
                    client.Send(sendingBuffer);                  
                }
                return;
            }

            if (dict.ContainsKey("nick") && dict.ContainsKey("email"))
            {
                //exist nick and email which can identify multiple profiles 
                //you have to add \more\bsr\******\final\
                error = SearchWithNickEmail(dict, out sendingBuffer);
                if (error != GPErrorCode.NoError)
                {
                    GameSpyUtils.SendGPError(client.Stream, error, sendingBuffer);
                }
                else
                {
                    client.Send(sendingBuffer);
                }
                return;
            }

            if(dict.ContainsKey("email"))
            {
                error = SearchWithEmail(dict, out sendingBuffer);
                if (error != GPErrorCode.NoError)
                {
                    GameSpyUtils.SendGPError(client.Stream, error, sendingBuffer);
                }
                else
                {
                    client.Send(sendingBuffer);
                }
                return;
            }
            //if above 3 functions not excuted then it must be parsing error
            GameSpyUtils.SendGPError(client.Stream, GPErrorCode.Parse, "There is a parse error in request!");

            //last one we search with email this may get few profile so we can not return GPErrorCode
            //SearchWithEmail(client,dict );
            //client.Stream.SendAsync(@"\bsr\1\nick\1\uniquenick\1\namespaceid\1\firstname\1\lastname\1\email\1\bsrdone\\more\bsr\2\nick\2\uniquenick\2\namespaceid\2\firstname\2\lastname\2\email\2\bsrdone\\final\");
            //we need full information to find a player.
            //if recieved data contians nick, uniquenick, email, 
            //a player must need to login to use the search function. sessionkey is 0 or else and profileid also.
            //case1:  uniquenick  email . check uniquenick email is exist or not
            //case4:  uniquenick   
            //case2:  nick uniquenick email 
            //case3:  nick uniquenick   
            //case5:  nick   email gamename
            //case6:  nick 
            //if not contain email, we need to
            //\search\\sesskey\0\profileid\0\namespaceid\1\partnerid\0\nick\mycrysis\uniquenick\xiaojiuwo\email\koujiangheng@live.cn\gamename\gmtest\final\
            //\bsrdone\more\<more>\final\
            //\bsr\
        }
        /// <summary>
        /// whether can be use when user not logged in
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private static GPErrorCode IsLogIn(Dictionary<string, string> dict)
        {
            if (dict["sesskey"] == "0" || dict["profileid"] == "0")
            {
                return GPErrorCode.General;
            }
            return GPErrorCode.NoError;
        }

        /// <summary>
        /// search with uniquenick
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="sendingBuffer"></param>
        /// <returns></returns>
        private static GPErrorCode SearchWithUniquenick(Dictionary<string, string> dict, out string sendingBuffer)
        {
            if (dict.ContainsKey("uniquenick") && dict["uniquenick"] != "0")
            {
                //TODO
                List<Dictionary<string, object>> temp =SearchQuery.GetProfileFromUniquenick(dict);
                if (temp.Count < 1)
                {
                    sendingBuffer = "No math found!";
                    return GPErrorCode.DatabaseError;
                }
                sendingBuffer =
                    string.Format(@"\bsr\{0}\nick\{1}\uniquenick\{2}\namespaceid\{3}\firstname\{4}\lastname\{5}\email\{6}\bsrdone\\final\",
                    temp[0]["profileid"], temp[0]["nick"], temp[0]["uniquenick"], temp[0]["namespaceid"], temp[0]["firstname"], temp[0]["lastname"], temp[0]["email"]);
                return GPErrorCode.NoError;
            }
            sendingBuffer = "No uniquenick found in the request!";
            return GPErrorCode.Parse;
        }
        /// <summary>
        /// search with nick and email
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="sendingBuffer"></param>
        /// <returns></returns>
        private static GPErrorCode SearchWithNickEmail(Dictionary<string, string> dict, out string sendingBuffer)
        {
            if (dict["email"] != "0" && dict["nick"] != "0'")
            {
                var temp = SearchQuery.GetProfileFromNickEmail(dict);
                
                if (temp==null)
                {
                    sendingBuffer = "No math found!";
                    return GPErrorCode.DatabaseError;
                }
                
                sendingBuffer =
               string.Format(@"\bsr\{0}\nick\{1}\uniquenick\{2}\namespaceid\{3}\firstname\{4}\lastname\{5}\email\{6}\bsrdone\\final\",
               temp[0]["profileid"], temp[0]["nick"], temp[0]["uniquenick"], temp[0]["namespaceid"], temp[0]["firstname"], temp[0]["lastname"], temp[0]["email"]);
                return GPErrorCode.NoError;

            }
            sendingBuffer = "Parse error in nick or email";
            return GPErrorCode.Parse;
        }
        /// <summary>
        /// only search with email, so we may get few profiles for one userid
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="sendingBuffer"></param>
        /// <returns></returns>
        private static GPErrorCode SearchWithEmail( Dictionary<string, string> dict, out string sendingBuffer)
        {
            if (dict["email"] != "0")
            {
                List<Dictionary<string, object>> temp = SearchQuery.GetProfileFromEmail(dict);
                if (temp.Count > 0)//we have multiple profiles 
                {
                    if (dict.ContainsKey("skip"))
                    {
                        int currentIndex = System.Convert.ToInt32(dict["skip"]);
                        if (currentIndex < temp.Count-1)
                        {
                            sendingBuffer = string.Format(
                       @"\bsr\{0}\nick\{1}\uniquenick\{2}\namespaceid\{3}\firstname\{4}\lastname\{5}\email\{6}\bsrdone\\more\final\",
                       temp[currentIndex]["profileid"], temp[currentIndex]["nick"], temp[currentIndex]["uniquenick"], temp[currentIndex]["namespaceid"], temp[currentIndex]["firstname"], temp[currentIndex]["lastname"], temp[currentIndex]["email"]);
                            return GPErrorCode.NoError;

                        }
                        if (currentIndex == (temp.Count - 1))
                        {
                            sendingBuffer = string.Format(
                         @"\bsr\{0}\nick\{1}\uniquenick\{2}\namespaceid\{3}\firstname\{4}\lastname\{5}\email\{6}\bsrdone\\final\",
                         temp[currentIndex]["profileid"], temp[currentIndex]["nick"], temp[currentIndex]["uniquenick"], temp[currentIndex]["namespaceid"], temp[currentIndex]["firstname"], temp[currentIndex]["lastname"], temp[currentIndex]["email"]);
                            return GPErrorCode.NoError;
                        }
                    }
                    else
                    {
                        if (temp.Count < 2)
                        {
                            sendingBuffer = string.Format(
                         @"\bsr\{0}\nick\{1}\uniquenick\{2}\namespaceid\{3}\firstname\{4}\lastname\{5}\email\{6}\bsrdone\\final\",
                         temp[0]["profileid"], temp[0]["nick"], temp[0]["uniquenick"], temp[0]["namespaceid"], temp[0]["firstname"], temp[0]["lastname"], temp[0]["email"]);
                            return GPErrorCode.NoError;
                        }
                        else
                        {
                            sendingBuffer = string.Format(
                        @"\bsr\{0}\nick\{1}\uniquenick\{2}\namespaceid\{3}\firstname\{4}\lastname\{5}\email\{6}\bsrdone\\more\final\",
                        temp[0]["profileid"], temp[0]["nick"], temp[0]["uniquenick"], temp[0]["namespaceid"], temp[0]["firstname"], temp[0]["lastname"], temp[0]["email"]);
                            return GPErrorCode.NoError;
                        }

                    }
                }
                sendingBuffer = "No match found!";
                return GPErrorCode.DatabaseError;
            }
            sendingBuffer = "Parse error in email";
            return GPErrorCode.Parse;
        }
    }
}
