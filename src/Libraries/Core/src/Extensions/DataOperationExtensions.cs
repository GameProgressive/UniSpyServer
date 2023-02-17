using System.Linq;
using UniSpy.Server.Core.Database.DatabaseModel;
namespace UniSpy.Server.Core.Extensions
{
    public class DataOperationExtensions
    {
        public static string GetSecretKey(string gameName)
        {
            using (var db = new UniSpyContext())
            {
                var result = from p in db.Games
                             where p.Gamename == gameName
                             select new { p.Secretkey };

                return result.First().Secretkey;
            }
        }
    }
}
