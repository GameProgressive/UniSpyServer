using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameSpyLib
{
    public delegate void OnExceptionEvent(Exception exception);

    public delegate void DataRecivedEvent(string Message);

    public delegate void ConnectionClosed();
}
