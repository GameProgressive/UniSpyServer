using System;

namespace GameSpyLib.Database.DatabaseException
{
    public class DbConnectException : Exception
    {
        public DbConnectException(string Message, Exception Inner) : base(Message, Inner) { }
    }
}
