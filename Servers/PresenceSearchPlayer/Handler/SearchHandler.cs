using GameSpyLib.Common;
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

            if (dict["profileid"] != "0")
            {
                List<Dictionary<string, object>> temp = GPSPHandler.DBQuery.GetProfileFromProfileid(dict);
                if (temp.Count < 1)
                {
                    GameSpyUtils.SendGPError(client.Stream, GPErrorCode.DatabaseError, "No math found!");
                    return;
                }
                sendingBuffer = 
                    string.Format(@"\bsr\{0}\nick\{1}\uniquenick{2}\namespaceid\{3}\firstname\{4}\lastname\{5}\email\{6}\bsrdone\0\final\",
                    dict["profileid"],temp[0]["nick"],temp[0]["uniquenick"],temp[0]["namespaceid"],temp[0]["firstname"],temp[0]["lastname"],temp[0]["email"]);

                client.Stream.SendAsync(sendingBuffer);
                return;
            }

            //if there is no profileid, we only need uniquenick
            if (dict.ContainsKey("uniquenick")&&dict["uniquenick"] != "0")
            {
                //TODO
                List<Dictionary<string,object>> temp = GPSPHandler.DBQuery.GetProfileFromUniquenick(dict);
                if (temp.Count < 1)
                {
                    GameSpyUtils.SendGPError(client.Stream, GPErrorCode.DatabaseError, "No math found!");
                    return;
                }
                sendingBuffer =
                    string.Format(@"\bsr\{0}\nick\{1}\uniquenick{2}\namespaceid\{3}\firstname\{4}\lastname\{5}\email\{6}\bsrdone\0\final\",
                    temp[0]["profileid"], temp[0]["nick"], temp[0]["uniquenick"], temp[0]["namespaceid"], temp[0]["firstname"], temp[0]["lastname"], temp[0]["email"]);
                client.Stream.SendAsync(sendingBuffer);
                return;
            }
            else
            {
                GameSpyUtils.SendGPError(client.Stream, GPErrorCode.Parse, "There is something missing in your search information");
            }

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
            string temp1 = @"\bsrdone\";
            string temp2 = @"more\"; //if value is 0 mean no more information, else do have informations



            //GameSpyUtils.PrintReceivedGPDictToLogger("search", dict);
            //GameSpyUtils.SendGPError(client.Stream, GPErrorCode.General, "This request is not supported yet.");
        }

        private static GPErrorCode IsLogIn(Dictionary<string, string> dict)
        {
            if (dict["sesskey"] == "0" || dict["profileid"] == "0")
            {
                return GPErrorCode.General;
            }
            return GPErrorCode.NoError;
        }


        private static void SearchWithUniquenick(Dictionary<string, string> dict)
        {
            //dict["nick"], dict["uniquenick"]; dict["email"] dict["namespaceid"] dict["partnerid"]
        }
    }
}
