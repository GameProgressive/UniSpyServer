using System.Collections.Generic;

namespace GameSpyLib.Database.DatabaseModel.MySql
{
    public partial class Partner
    {
        public Partner()
        {
            Subprofiles = new HashSet<Subprofiles>();
        }

        public uint Partnerid { get; set; }
        public string Partnername { get; set; }

        public virtual ICollection<Subprofiles> Subprofiles { get; set; }
    }
}
