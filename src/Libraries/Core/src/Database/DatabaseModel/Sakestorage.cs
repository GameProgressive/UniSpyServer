using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniSpy.Server.Core.Database.DatabaseModel
{
    /// <summary>
    /// Sake storage system.
    /// </summary>
    public partial class Sakestorage
    {
        public int Sakestorageid { get; set; }
        public string Tableid { get; set; } = null!;
        public int Profileid { get; set; }
        [Column(TypeName = "jsonb")]
        public Dictionary<string, string> Userdata { get; set; } = null!;

        public virtual Profile Profile { get; set; } = null!;
    }
}


