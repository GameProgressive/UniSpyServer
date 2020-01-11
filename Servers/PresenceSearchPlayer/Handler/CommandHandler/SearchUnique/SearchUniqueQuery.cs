using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer.Handler.CommandHandler.SearchUnique
{
    public class SearchUniqueQuery
    {
        public static Dictionary<string,object>GetProfileWithUniquenickAndNamespace(string uniquenick,uint namespaceid)
        {
            var result = GPSPServer.DB.Query(
                                                                        @"SELECT profiles.profileid,
                                                                        profiles.nick,
                                                                        namespace.uniquenick,
                                                                        profiles.lastname,
                                                                        profiles.firstname,
                                                                        users.email,
                                                                        namespace.namespaceid 
                                                                        FROM profiles 
                                                                        INNER JOIN users ON users.userid = profiles.userid 
                                                                        INNER JOIN namespace ON namespace.profileid = profiles.profileid 
                                                                        WHERE namespace.uniquenick=@P0 AND namespace.namespaceid = @P1",
                                                                        uniquenick,namespaceid
                                                                    );
            return (result.Count == 0) ? null : result[0];
        }
    }
}
