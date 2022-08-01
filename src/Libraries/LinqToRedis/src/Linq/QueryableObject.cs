using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace UniSpyServer.LinqToRedis.Linq
{
    public class QueryableObject<T> : IQueryable<T>
    {
        private QueryProviderBase _provider;
        private Expression _expression;
        public QueryableObject(QueryProviderBase provider)
        {
            if (provider is null)
            {
                throw new ArgumentNullException("provider");
            }
            _provider = provider;
            _expression = Expression.Constant(this);
        }
        public QueryableObject(QueryProviderBase provider, Expression expression)
        {
            if (provider is null)
            {
                throw new ArgumentNullException("provider");
            }

            if (expression is null)
            {
                throw new ArgumentNullException("expression");
            }

            if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentOutOfRangeException("expression");
            }

            _provider = provider;
            _expression = expression;
        }
        public Type ElementType => typeof(T);
        public Expression Expression => _expression;
        public IQueryProvider Provider => _provider;

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_provider.Execute(_expression)).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}