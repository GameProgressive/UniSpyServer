using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Handler.Command.CRYPT
{
    public class CRYPTQuery
    {
        public static Dictionary<string, object> GetSecretKeyFromGame(string gameName)
        {
            var result = ChatServer.DB.Query(
                @"SELECT secretkey FROM games WHERE gamename=@P0"
                 , gameName
                 );
            return (result.Count == 0) ? null : result[0];
        }
    }
}
