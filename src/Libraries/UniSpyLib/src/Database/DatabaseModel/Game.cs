using System.Collections.Generic;

namespace UniSpyServer.UniSpyLib.Database.DatabaseModel
{
    /// <summary>
    /// Game list.
    /// </summary>
    public partial class Game
    {
        public Game()
        {
            Grouplists = new HashSet<Grouplist>();
        }

        public int Gameid { get; set; }
        public string Gamename { get; set; } = null!;
        public string Secretkey { get; set; }
        public string Description { get; set; } = null!;
        public bool Disabled { get; set; }

        public virtual ICollection<Grouplist> Grouplists { get; set; }
    }
}
