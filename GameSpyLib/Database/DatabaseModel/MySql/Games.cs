using System.Collections.Generic;

namespace GameSpyLib.Database.DatabaseModel.MySql
{
    public partial class Games
    {
        public Games()
        {
            Grouplist = new HashSet<Grouplist>();
        }

        public uint Gameid { get; set; }
        public string Gamename { get; set; }
        public string Secretkey { get; set; }
        public string Description { get; set; }
        public uint Queryport { get; set; }
        public bool Disabled { get; set; }

        public virtual ICollection<Grouplist> Grouplist { get; set; }
    }
}
