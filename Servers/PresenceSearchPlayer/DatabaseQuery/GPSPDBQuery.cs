using GameSpyLib.Database;
using System;
using System.Collections.Generic;

namespace PresenceSearchPlayer
{
    public class GPSPDBQuery 
    {



       

        


        public List<Dictionary<string, object>> GetProfileFromProfileid(Dictionary<string, string> dict)
        {
            return GPSPServer.DB.Query("SELECT nick,uniquenick,lastname,firstname,email,namespaceid FROM profiles,users,namespace WHERE profiles.profileid=@P0", dict["profileid"]);
        }

       
      
      

       
        

        public uint GetprofileidFromEmail(string email)
        {
            uint profileid;
            List<Dictionary<string, object>> temp = GPSPServer.DB.Query("SELECT profileid FROM profiles " +
                "INNER JOIN users ON users.userid=profiles.userid " +
                "WHERE LOWER(users.email)=@P0", email);
            if (temp.Count > 0)
            {
                profileid = (uint)temp[0]["profileid"];
                return profileid;
            }
            else
            {
                //todo
                profileid = 1;
                return profileid;
                throw new NotImplementedException();
            }
        }

        public uint CreateUserWithUnique(Dictionary<string, string> dict, uint userid)
        {
            GPSPServer.DB.Execute("INSERT INTO profiles(userid,nick) VALUES (@P0,@P1)", userid, dict["nick"]);
            uint profileid = (uint)GPSPServer.DB.Query("SELECT profileid FROM profiles INNER JOIN users WHERE profiles.userid=users.userid AND profiles.nick = @P0 AND profiles.userid = @P1", dict["nick"], userid)[0]["profileid"];
            GPSPServer.DB.Execute("INSERT INTO namespace(profileid,uniquenick,namespaceid,partnerid,productid) VALUES (@P0,@P1,@P2,@P3,@P4)", profileid, dict["uniquenick"], dict["namespaceid"], dict["partnerid"], dict["productID"]);
            return profileid;
        }



       
    }
}
