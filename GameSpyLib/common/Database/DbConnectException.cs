using System;

namespace GameSpyLib
{
    public class DbConnectException : Exception
    {
        public DbConnectException(string Message, Exception Inner) : base(Message, Inner) { }
    }
}
