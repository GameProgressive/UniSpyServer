using System;
using System.Collections.Generic;

namespace GameSpyLib.Database.DatabaseModel.MySql
{
    public partial class Games
    {
        public int Id { get; set; }
        public string Gamename { get; set; }
        public string Secretkey { get; set; }
        public string Description { get; set; }
        public int Queryport { get; set; }
        public int Backendflags { get; set; }
        public int Disabledservices { get; set; }
        public string Keylist { get; set; }
        public string Keytypelist { get; set; }
    }
}
