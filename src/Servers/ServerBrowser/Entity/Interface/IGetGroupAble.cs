using GameSpyLib.Database.DatabaseModel.MySql;
using System.Collections.Generic;

namespace ServerBrowser.Entity.Interface
{
    public interface IGetGroupAble
    {
        public IEnumerable<Grouplist> GetAvailableGroup(string gameName);
    }
}
