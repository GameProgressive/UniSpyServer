
using GameSpyLib.Database;

namespace RetroSpyServer.DBQueries
{
    public class CDKeyDBQuery : DBQueryBase
    {

        public CDKeyDBQuery(DatabaseDriver dbdriver) : base(dbdriver)
        {
        }

        public bool IsCDKeyValidate(string str)
        {
            //To Do
            return true;
        }
    }
}
