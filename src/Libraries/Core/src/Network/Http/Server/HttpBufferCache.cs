namespace UniSpy.Server.Core.Network.Http.Server
{
    public class HttpBufferCache : UniSpy.Server.Core.Misc.BufferCacheBase<string>
    {
        public HttpBufferCache()
        {
        }

        public override bool ProcessBuffer(string buffer, out string completeBuffer)
        {
            if (buffer.Contains("</SOAP-ENV:Envelope>"))
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