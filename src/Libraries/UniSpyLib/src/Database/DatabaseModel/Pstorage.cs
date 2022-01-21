using System;
using System.Collections.Generic;

namespace UniSpyServer.UniSpyLib.Database.DatabaseModel
{
    /// <summary>
    /// Old games use pstorage to store game data.
    /// </summary>
    public partial class Pstorage
    {
        public int Pstorageid { get; set; }
        public int ProfileId { get; set; }
        public int Ptype { get; set; }
        public int Dindex { get; set; }
        public string Data { get; set; }

        public virtual Profile Profile { get; set; } = null!;
    }
}
