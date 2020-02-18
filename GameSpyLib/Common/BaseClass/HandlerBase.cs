namespace GameSpyLib.Common
{
    public abstract class HandlerBase<T1, T2>
    {
        public abstract void Handle(T1 source);
    }
}
