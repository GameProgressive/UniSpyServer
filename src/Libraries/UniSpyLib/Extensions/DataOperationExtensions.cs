using System.Linq;
using UniSpyLib.Database.DatabaseModel.MySql;
namespace UniSpyLib.Extensions
{
    public class DataOperationExtensions
    {
        public static string GetSecretKey(string gameName)
        {
            using (var db = new unispyContext())
            {
                var result = from p in db.Games
                             where p.Gamename == gameName
                             select new { p.Secretkey };

                return result.First().Secretkey;
            }
        }
    }
}
