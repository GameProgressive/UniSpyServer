using UniSpyLib.Database.DatabaseModel.MySql;
using System.Collections.Generic;

namespace ServerBrowser.Abstraction.Interface
{
    public interface IGetGroupAble
    {
        public IEnumerable<Grouplist> GetAvailableGroup(string gameName);
    }
}
