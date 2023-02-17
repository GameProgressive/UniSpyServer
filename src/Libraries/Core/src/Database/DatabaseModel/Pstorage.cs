using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniSpy.Server.Core.Database.DatabaseModel
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
        [Column(TypeName = "jsonb")]
        public Dictionary<string, string> Data { get; set; }
        // public string Data { get; set; }
        public virtual Profile Profile { get; set; } = null!;
    }
}
