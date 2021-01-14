using System.Collections.Generic;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace ServerBrowser.Abstraction.Interface
{
    public interface IGetGroupAble
    {
        public IEnumerable<Grouplist> GetAvailableGroup(string gameName);
    }
}
