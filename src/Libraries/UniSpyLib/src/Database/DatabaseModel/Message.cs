using System;

namespace UniSpyServer.UniSpyLib.Database.DatabaseModel
{
    /// <summary>
    /// Friend messages.
    /// </summary>
    public partial class Message
    {
        public int Messageid { get; set; }
        public int? Namespaceid { get; set; }
        public int? Type { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public DateTime Date { get; set; }
        public string Message1 { get; set; } = null!;
    }
}
