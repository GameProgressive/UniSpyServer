namespace UniSpy.Server.Core.Misc
{
    public abstract class BufferCacheBase<T>
    {
        public T InCompleteBuffer { get; protected set; }

        protected BufferCacheBase()
        {
        }

        /// <summary>
        /// Process the incoming buffer, if buffer is complete return true, otherwise return false
        /// </summary>
        /// <param name="buffer"></param>
        public abstract bool ProcessBuffer(T buffer, out T completeBuffer);
    }
}