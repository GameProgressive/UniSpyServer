using System.Linq;

namespace UniSpy.Server.Chat.Aggregate.Misc
{
    public class BufferCache : Core.Misc.BufferCacheBase<byte[]>
    {
        public BufferCache()
        {
        }

        public override bool ProcessBuffer(byte[] buffer, out byte[] completeBuffer)
        {
            if (buffer[buffer.Length - 1] == 0x0A)
            {
                // check last _incomplteBuffer if it has incomplete message, then combine them
                if (InCompleteBuffer is not null)
                {
                    completeBuffer = InCompleteBuffer.Concat(buffer).ToArray();
                    InCompleteBuffer = null;
                }
                else
                {
                    completeBuffer = buffer;
                }
                return true;
            }
            else
            {
                // message is not finished, we add it in _completeBuffer
                if (InCompleteBuffer is null)
                {
                    InCompleteBuffer = buffer;
                }
                else
                {
                    InCompleteBuffer = InCompleteBuffer.Concat(buffer).ToArray();
                }
                completeBuffer = null;
                return false;
            }
        }
    }
}