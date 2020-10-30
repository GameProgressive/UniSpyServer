namespace GameSpyLib.Abstraction.Interface
{
    public interface ISession
    {
        public object GetInstance();

        public long Send(byte[] buffer, long offset, long size);
        public long Send(string text);
        public long Send(byte[] buffer);

        public bool SendAsync(byte[] buffer, long offset, long size);
        public bool SendAsync(string text);
        public bool SendAsync(byte[] buffer);

        public bool BaseSendAsync(string buffer);
        public bool BaseSendAsync(byte[] buffer);
    }
}
