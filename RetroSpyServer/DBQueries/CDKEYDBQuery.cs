
using GameSpyLib.Database;

namespace RetroSpyServer.DBQueries
{
    public class CDKEYDBQuery:DBQueryBase
    {
        public CDKEYDBQuery() : base()
        {
        }
        public CDKEYDBQuery(DatabaseDriver dbdriver) : base(dbdriver)
        {
        }

        public bool CheckCDKEYValidate()
        {
            //To Do
            return true;
        }
    }
}
