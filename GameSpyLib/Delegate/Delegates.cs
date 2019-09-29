using System;

namespace GameSpyLib.Delegate
{
    public delegate void OnExceptionEvent(Exception exception);

    public delegate void DataRecivedEvent(string Message);

    public delegate void ConnectionClosed();

    public delegate bool MessageFinished(string Message);
}
