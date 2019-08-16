
using GameSpyLib.Database;

namespace CDKey
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
