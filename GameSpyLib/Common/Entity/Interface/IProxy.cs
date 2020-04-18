namespace GameSpyLib.Common.Entity.Interface
{
    public interface IProxy
    {
        public long Send(byte[] buffer, long offset, long size);
        public long Send(string text);
        public long Send(byte[] buffer);

        public bool SendAsync(byte[] buffer, long offset, long size);
        public bool SendAsync(string text);
        public bool SendAsync(byte[] buffer);

        public bool BaseSendAsync(string buffer);
        public bool BaseSendAsync(byte[] buffer);
        public bool BaseSendAsync(byte[] buffer, long offset, long size);
    }
}
