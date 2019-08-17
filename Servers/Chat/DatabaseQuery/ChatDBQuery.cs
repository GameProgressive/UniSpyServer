using System;
using System.Collections.Generic;
using System.Text;
using GameSpyLib.Database;

namespace Chat.DBQueries
{
    public class ChatDBQuery : DBQueryBase
    {
        public ChatDBQuery(DatabaseDriver dbdriver) : base(dbdriver)
        {
        }
    }
}
