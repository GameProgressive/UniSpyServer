using System;
using System.Linq;
using System.Linq.Expressions;

namespace UniSpyServer.LinqToRedis.Linq
{
    public abstract class QueryProviderBase : IQueryProvider
    {
        public QueryProviderBase() { }
        IQueryable<T> IQueryProvider.CreateQuery<T>(Expression expression) => new QueryableObject<T>(this, expression);
        IQueryable IQueryProvider.CreateQuery(Expression expression) => (IQueryable)Activator.CreateInstance(typeof(QueryableObject<>).MakeGenericType(expression.Type), new object[] { this, expression });
        T IQueryProvider.Execute<T>(Expression expression) => (T)Execute(expression);
        object IQueryProvider.Execute(Expression expression) => Execute(expression);
        public abstract object Execute(Expression expression);
    }
}