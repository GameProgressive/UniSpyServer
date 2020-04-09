using System;
using GameSpyLib.Database.DatabaseModel.MySql;
using System.Linq;
namespace GameSpyLib.Extensions
{
    public class DataOperationExtensions
    {
        public static bool GetSecretKey(string gameName, out string secretKey)
        {
            using (var db = new retrospyContext())
            {
                var result = from p in db.Games
                             where p.Gamename == gameName
                             select new { p.Secretkey };

                if (result.Count() == 1 && result.First() != null)
                {
                    secretKey = result.First().Secretkey;
                    return true;
                }
                else
                {
                    secretKey = null;
                    return false;
                }
            }
        }
    }
}
