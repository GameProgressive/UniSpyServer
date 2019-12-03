using System;

namespace GameSpyLib.Database
{
    public class DbConnectException : Exception
    {
        public DbConnectException(string Message, Exception Inner) : base(Message, Inner) { }
    }
}
