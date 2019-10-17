using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Handler.GetProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresenceConnectionManager.Handler.SendBuddies
{
    /// <summary>
    /// Send friendlist, friends message, friends add request to player when he logged in.
    /// </summary>
    public class SendBuddiesHandler
    {

        //**********************************************************
        //\bm\<MESSAGE>\f\<from profileid>\msg\<>\final\
        //\bm\<UTM>\f\<from profileid>\msg\<>\final\
        //\bm\<REQUEST>\f\<from profileid>\msg\|signed|\final\
        //\bm\<AUTH>\f\<from profileid>\final\
        //\bm\<REVOKE>\f\<from profileid>\final\
        //\bm\<STATUS>\f\<from profileid>\msg\|s|<status code>|ss|<status string>|ls|<location string>|ip|<>|p|<port>|qm|<quiet mode falgs>\final\
        //\bm\<INVITE>\f\<from profileid>\msg\|p|<productid>|l|<location string>
        //\bm\<ping>\f\<from profileid>\msg\final\
        //\bm\<pong>\f\<from profileid>\final\

        public static void SendBuddyList(GPCMSession session)
        {
            // \bdy\<number of friends>\list\<array of profileids>\
            //total number of friends
            // we have to separate friends by productid,namespaceid,partnerid,gamename 
            //because you will have different friends in different game            

            if (session.PlayerInfo.BuddiesSent)
                return;
            session.PlayerInfo.BuddiesSent = true;
            //return;
            string sendingBuffer = @"\bdy\";
            var result = SendBuddiesQuery.SearchBuddiesId(session.PlayerInfo.Profileid, session.PlayerInfo.Namespaceid);
            uint[] profileids;
            if (result != null)
            {
                //convert the object in dictionary to uint array
                profileids= result.Values.Cast<uint>().ToArray();                
                sendingBuffer += result.Count + @"\list\";
                for(int i=0;i<profileids.Length;i++)
                {
                    sendingBuffer += profileids[i] + @",";
                    //last one we do not add ,
                    if (i == profileids.Length - 1)
                    {
                        sendingBuffer += profileids[i];
                    }
                }
                foreach (KeyValuePair<string, object> id in result)
                {
                    sendingBuffer += Convert.ToUInt32(id.Value) + @",";
                    if (id.Equals(result.Last()))
                    {
                        sendingBuffer += Convert.ToUInt32(id.Value);
                    }
                }
                sendingBuffer += @"\final\";
                session.SendAsync(sendingBuffer);
                //we send the player's status info to client
                Task.Run(() => SendBuddyStatusInfo(session, profileids));
            }
            else
            {
                sendingBuffer = @"\bdy\0\list\\final\";
                session.SendAsync(sendingBuffer);
            }
          
          


        }
        public static void SendBuddyStatusInfo(GPCMSession session, uint[] profileids)
        {
            Dictionary<string, object> result;

            foreach (uint profileid in profileids)
            {
                string sendingBuffer = @"\bm\" + GPEnum.BmStatus + @"\f\";
                result = SendBuddiesQuery.GetProfile(profileid, session.PlayerInfo.Namespaceid);
                sendingBuffer += profileid + @"\msg\";
                sendingBuffer += @"|s|" + Convert.ToUInt32(result["statuscode"]);
                sendingBuffer += @"|ss|" + result["status"].ToString();
                sendingBuffer += @"|ls|" + result["location"].ToString();
                sendingBuffer += @"|ip|" + result["lastip"];
                sendingBuffer += @"|p|" + Convert.ToUInt32(result["port"]);
                sendingBuffer += @"|qm|" + result["quietflags"] + @"\final\";

                session.SendAsync(sendingBuffer);
            }
        }


        public static void SendBuddyMessage(GPCMSession session, uint profileid)
        { }

        public static void SendBuddyUTM(GPCMSession session, uint profileid)
        { }

        public static void SendBuddyAddRequest()
        {
            //char query[256];
            //sprintf_s(query, sizeof(query), "SELECT `profileid`,`syncrequested`,`reason` FROM `Presence`.`addrequest` WHERE `targetid` = %d", getProfileID());
            //MYSQL_RES* res;
            //MYSQL_ROW row;
            //sentAddRequests = true;
            //if (mysql_query(server.conn, query))
            //{
            //    fprintf(stderr, "%s\n", mysql_error(server.conn));
            //    return;
            //}
            //res = mysql_store_result(server.conn);
            //while ((row = mysql_fetch_row(res)) != NULL)
            //{
            //    formatSend(sd, true, 0, "\\bm\\%d\\f\\%d\\msg\\%s|signed|%s", GPI_BM_REQUEST, atoi(row[0]), row[2], row[1]);
            //    //formatSend(c->sd,true,0,"\\bm\\%d\\f\\%d\\msg\\%s|signed|%s",GPI_BM_REQUEST,profileid,reason,signature);
            //}
            //mysql_free_result(res);
        }
        public static void BuddyAuth()
        { }
    }
}
