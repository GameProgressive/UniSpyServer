using GameSpyLib.Database.DatabaseModel.MySql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace Chat.Handler.CommandHandler.CRYPT
{
    public class CRYPTQuery
    {
        public static string GetSecretKeyFromGame(string gameName)
        {
            using (var db = new RetrospyDB())
            {
                var secretkey = from g in db.Games
                                   where g.Gamename == gameName
                                   select g.Secretkey;
                return secretkey.Count()==0? null: secretkey.First();               
            
            }
        }
    }
}
