using System;
using System.Collections.Generic;
using System.Net;

namespace UniSpy.Server.Core.Database.DatabaseModel
{
    /// <summary>
    /// User account information.
    /// </summary>
    public partial class User
    {
        public User()
        {
            Profiles = new HashSet<Profile>();
        }

        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool? Emailverified { get; set; }
        public IPAddress LastIp { get; set; }
        public DateTime Lastonline { get; set; }
        public DateTime Createddate { get; set; }
        public bool Banned { get; set; }
        public bool Deleted { get; set; }

        public virtual ICollection<Profile> Profiles { get; set; }
    }
}
