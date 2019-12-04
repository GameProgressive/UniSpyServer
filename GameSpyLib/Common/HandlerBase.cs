using System;
using System.Collections.Generic;
using System.Text;

namespace GameSpyLib.Common
{
    public abstract class HandlerBase
    {
        private static Dictionary<string, string> _recv;
        /// <summary>
        /// store the query result from database
        /// </summary>
        private static Dictionary<string, object> _queryResult;
        /// <summary>
        /// store the information we need to send to user
        /// </summary>
        private static string _sendingBuffer;

        private static string _errorMsg;
    }
}
