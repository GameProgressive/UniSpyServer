
using GameSpyLib.Database;

namespace RetroSpyServer.DBQueries
{
    public class CDKEYDBQuery : DBQueryBase
    {
        public CDKEYDBQuery() : base()
        {
        }
        public CDKEYDBQuery(DatabaseDriver dbdriver) : base(dbdriver)
        {
        }

        public bool IsCDKeyValidate(string str)
        {
            //To Do
            return true;
        }
    }
}
