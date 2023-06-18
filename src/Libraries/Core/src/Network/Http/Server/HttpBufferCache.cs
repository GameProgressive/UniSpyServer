
using System.Linq;
using UniSpy.Server.Core.Encryption;

namespace UniSpy.Server.Core.Network.Http.Server
{
    public class HttpBufferCache : UniSpy.Server.Core.Misc.BufferCacheBase<string>
    {
        private const string _endFlag = "</SOAP-ENV:Envelope>";
        public HttpBufferCache()
        {
        }

        public override bool ProcessBuffer(string buffer, out string completeBuffer)
        {
            if (buffer.Contains(_endFlag))
            {
                if (InCompleteBuffer is null)
                {
                    completeBuffer = buffer;
                    return true;
                }
                else
                {
                    completeBuffer = InCompleteBuffer + buffer;
                    InCompleteBuffer = null;
                    return true;
                }
            }
            else
            {
                if (InCompleteBuffer is null)
                {
                    InCompleteBuffer = buffer;
                    completeBuffer = null;
                    return false;
                }
                else
                {
                    InCompleteBuffer += buffer;
                    completeBuffer = null;
                    return false;
                }
            }
        }
    }
}