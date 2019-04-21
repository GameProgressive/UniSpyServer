namespace System.Collections.Generic
{
    /// <summary>
    /// This is a very nice dictionary, because it throws a NiceKeyNotFoundException that
    /// tells us the key that was not found. Thank you, nice dictionary!
    /// </summary>
    public class NiceDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public new TValue this[TKey key]
        {
            get
            {
                try
                {
                    return base[key];
                }
                catch (KeyNotFoundException knfe)
                {
                    throw new NiceKeyNotFoundException<TKey>(key, knfe.Message + ": \"" + key + "\"", knfe.InnerException);
                }
            }
            set
            {
                try
                {
                    base[key] = value;
                }
                catch (KeyNotFoundException knfe)
                {
                    throw new NiceKeyNotFoundException<TKey>(key, knfe.Message + ": \"" + key + "\"", knfe.InnerException);
                }
            }
        }

        // Constructors
        public NiceDictionary() : base() { }
        public NiceDictionary(int capacity) : base(capacity) { }
        public NiceDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }
        public NiceDictionary(IEqualityComparer<TKey> comparer) : base(comparer) { }
        public NiceDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary, comparer) { }
        public NiceDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer) { }
    }

    /// <summary>
    /// This is a nice variant of the KeyNotFoundException. The original version 
    /// is very mean, because it refuses to tell us which key was responsible 
    /// for raising the exception.
    /// </summary>
    public class NiceKeyNotFoundException<TKey> : KeyNotFoundException
    {
        public TKey Key { get; private set; }

        public NiceKeyNotFoundException(TKey key, string message) : base(message, null)
        {
            this.Key = key;
        }

        public NiceKeyNotFoundException(TKey key, string message, Exception innerException) : base(message, innerException)
        {
            this.Key = key;
        }
    }
}
