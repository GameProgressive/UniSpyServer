using System;
using System.Collections.Generic;

namespace UniSpyServer.UniSpyLib.Database.DatabaseModel
{
    /// <summary>
    /// Friend request.
    /// </summary>
    public partial class Addrequest
    {
        public int Addrequestid { get; set; }
        public int ProfileId { get; set; }
        public int Namespaceid { get; set; }
        public int Targetid { get; set; }
        public string Reason { get; set; } = null!;
        public string Syncrequested { get; set; } = null!;

        public virtual Profile Profile { get; set; } = null!;
    }
}
