using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler
{
    public class PmatchHandler
    {
        public static void SeachPlayers(GPSPClient client, Dictionary<string, string> dict)
        {
            string sendingBuffer1, sendingBuffer2;
            sendingBuffer1 = @"\psrdone\";
            sendingBuffer2 = @"\psr\";
            //there are two ways to send information back.

            //First way: \psr\<profileid>\status\<status>\statuscode\<statuscode>\psrdone\final\

            //this is a multiple command. you can contain mutiple \psr\........... in the Steam
            //Second way:\psr\<profileid>\nick\<nick>\***multiple \psr\ command***\psrdone\final\
            //<status> is like the introduction in a player homepage
            //<statuscode> mean the status information is support or not the value should be as follows
            //GP_NEW_STATUS_INFO_SUPPORTED = 0xC00,
            //GP_NEW_STATUS_INFO_NOT_SUPPORTED = 0xC01


            sendingBuffer2 += @"status\";

            GameSpyUtils.PrintReceivedGPDictToLogger("pmatch", dict);
            GameSpyUtils.SendGPError(client.Stream, GPErrorCode.General, "This request is not supported yet.");
        }

    }
}
