using GameSpyLib.Database;


namespace RetroSpyServer.DBQueries
{
    public class MasterDBQuery : DBQueryBase
    {
        public static GPCMDBQuery DBQuery = null;
        public MasterDBQuery(DatabaseDriver dbdriver) : base(dbdriver)
        {
        }

       
    }
}
