using System;
namespace GameSpyLib.Common
{
    public abstract class HandlerBase<T1, T2>
    {
        public virtual void Handle(T1 source) { }
    }
}
