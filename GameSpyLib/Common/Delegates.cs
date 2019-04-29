using System;
using GameSpyLib.Network;

namespace GameSpyLib.Common
{
    public delegate void OnExceptionEvent(Exception exception);

    public delegate void DataRecivedEvent(string Message);

    public delegate void ConnectionClosed();
}
