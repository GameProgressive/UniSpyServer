using System;
using System.Collections.Generic;
using System.Linq;
using GameSpyLib.Database.DatabaseModel.MySql;

namespace ServerBrowser.Entity.Interface
{
    public interface IGetGroupAble
    {
        public IEnumerable<Grouplist> GetAvailableGroup(string gameName);
    }
}
