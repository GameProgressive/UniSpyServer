using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.Pmatch
{
    public class PmatchHandler
    {
        public static void PlayerMatch(GPSPSession session, Dictionary<string, string> dict)
        { //pmath\\sesskey\\profileid\\productid\\
            string sendingBuffer;
            if (IsContainAllKey(dict))
            {
                List<Dictionary<string, object>> temp = PmatchQuery.PlayerMatch(dict);
                if (temp.Count == 1)
                {
                    sendingBuffer = string.Format(@"\psr\status\{0}\nick\{1}\statuscode\{2}\final\",
                        temp[0]["status"], temp[0]["nick"], temp[0]["statuscode"]);
                    session.SendAsync(sendingBuffer);
                }
                else
                {
                    GameSpyUtils.SendGPError(session, GPErrorCode.DatabaseError, "No match found!");
                }

            }

            //there are two ways to send information back.

            //First way: \psr\<profileid>\status\<status>\statuscode\<statuscode>\psrdone\final\

            //this is a multiple command. you can contain mutiple \psr\........... in the Steam
            //Second way:\psr\<profileid>\nick\<nick>\***multiple \psr\ command***\psrdone\final\
            //<status> is like the introduction in a player homepage
            //<statuscode> mean the status information is support or not the value should be as follows
            //GP_NEW_STATUS_INFO_SUPPORTED = 0xC00,
            //GP_NEW_STATUS_INFO_NOT_SUPPORTED = 0xC01            
        }

        private static bool IsContainAllKey(Dictionary<string, string> dict)
        {
            if (dict.ContainsKey("sesskey") && dict.ContainsKey("profileid") && dict.ContainsKey("productid"))
            {
                if (GameSpyUtils.IsNumber(dict["sesskey"]) && GameSpyUtils.IsNumber(dict["profileid"]) && GameSpyUtils.IsNumber(dict["productid"]))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
    }
}
