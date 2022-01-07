using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace UniSpyServer.LinqToRedis.Linq
{
    public class RedisQueryProvider<TValue> : QueryProviderBase where TValue : RedisKeyValueObject
    {
        private RedisClient<TValue> _client;
        public RedisQueryProvider(RedisClient<TValue> client) : base()
        {
            _client = client;
        }

        public override object Execute(Expression expression)
        {
            var matchedKeys = new List<string>();
            expression = QueryEvaluator.PartialEval(expression);
            var builder = new RedisQueryBuilder<TValue>(expression);
            builder.Build();
            var values = _client.GetValues(builder.KeyObject);
            var node = (MethodCallExpression)expression;
            switch (node.Method.Name)
            {
                case "Where":
                    return values;
                case "FirstOrDefault":
                    return values.FirstOrDefault();
                case "First":
                    return values.First();
                default:
                    throw new NotSupportedException(string.Format("The method '{0}' is not supported", node.Method.Name));
            }
        }
    }
}