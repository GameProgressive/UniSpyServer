using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Handler.Command.CRYPT
{
    public class CRYPTQuery
    {
        public static string GetSecretKeyFromGame(string gameName)
        {
            var result = ChatServer.DB.Query(
                @"SELECT secretkey FROM games WHERE gamename=@P0"
                 , gameName
                 );
            return (result.Count == 0) ? null : result[0]["secretkey"].ToString();
        }
    }
}
