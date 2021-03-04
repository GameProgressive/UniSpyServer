using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Abstraction.Interface;

namespace UniSpyLib.Abstraction.BaseClass
{
    public class UniSpyConcurrentDictionary<TKey, TSession> : ConcurrentDictionary<TKey, TSession>, IGrouping<TKey,TSession>
    {
        public UniSpyConcurrentDictionary()
        {
        }

        public TKey Key => Key;

        IEnumerator<TSession> IEnumerable<TSession>.GetEnumerator()
        {
            return (IEnumerator<TSession>)Values;
        }
    }
}
