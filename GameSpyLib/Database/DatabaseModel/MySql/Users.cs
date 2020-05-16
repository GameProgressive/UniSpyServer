using System;
using System.Collections.Generic;

namespace GameSpyLib.Database.DatabaseModel.MySql
{
    public partial class Users
    {
        public Users()
        {
            Profiles = new HashSet<Profiles>();
        }

        public uint Userid { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? Emailverified { get; set; }
        public string Lastip { get; set; }
        public DateTime? Lastonline { get; set; }
        public DateTime Createddate { get; set; }
        public bool Banned { get; set; }
        public bool Deleted { get; set; }

        public virtual ICollection<Profiles> Profiles { get; set; }
    }
}
