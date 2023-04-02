using System.Linq;
using UniSpy.Server.ServerBrowser.V2.Enumerate;

namespace UniSpy.Server.ServerBrowser.V2.Aggregate.Misc
{
    public class BufferCache : Core.Misc.BufferCacheBase<byte[]>
    {
        public override bool ProcessBuffer(byte[] buffer, out byte[] completeBuffer)
        {
            if (((RequestType)buffer[2]) == RequestType.SendMessageRequest)
            {
                if (buffer.Length > 9)
                {
                    // complete sendmessage request received
                    completeBuffer = buffer;
                    return true;
                }
                else
                {
                    InCompleteBuffer = buffer;
                    completeBuffer = null;
                    return false;
                }
            }
            else if (buffer.Take(6).SequenceEqual(NatNegotiation.Abstraction.BaseClass.RequestBase.MagicData))
            {
                if (InCompleteBuffer is not null)
                {
                    completeBuffer = InCompleteBuffer.Concat(buffer).ToArray();
                    InCompleteBuffer = null;
                    return true;
                }
                else
                {
                    // we ignore natneg message when _incompleteBuffer is null
                    completeBuffer = null;
                    return false;
                }
            }
            else
            {
                completeBuffer = buffer;
                return true;
            }
        }
    }
}