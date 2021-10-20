using System;

namespace UniSpyServer.UniSpyLib.Database.DatabaseModel.MySql
{
    public partial class Messages
    {
        public uint Messageid { get; set; }
        public uint? Namespaceid { get; set; }
        public uint? Type { get; set; }
        public uint From { get; set; }
        public uint To { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
    }
}
