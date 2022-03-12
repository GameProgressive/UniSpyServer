using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace UniSpyServer.LinqToRedis.Linq
{
    /// <summary>
    /// Provide high level API for redis query building
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class RedisQueryBuilder<TKey> : ExpressionVisitor where TKey : RedisKeyValueObject
    {
        public TKey KeyObject { get; private set; }
        private List<object> _builderStack = new List<object>();
        private Expression _expression;
        public RedisQueryBuilder(Expression expression)
        {
            _expression = expression;
        }

        public void Build()
        {
            KeyObject = (TKey)Activator.CreateInstance(typeof(TKey));
            Visit(_expression);
            for (int i = 0; i < _builderStack.Count; i += 2)
            {
                var keyProperty = KeyObject.GetType().GetProperty((string)_builderStack[i]);
                // get target type
                var targetType = IsNullableType(keyProperty.PropertyType) ? Nullable.GetUnderlyingType(keyProperty.PropertyType) : keyProperty.PropertyType;
                // convert value to target type
                var value = _builderStack[i + 1];
                // _builderStack[i + 1] = Convert.ChangeType(_builderStack[i + 1], targetType);
                if (targetType.IsEnum)
                {
                    value = Enum.ToObject(targetType, value);
                }
                else
                {
                    value = Convert.ChangeType(value, targetType);
                }
                keyProperty.SetValue(KeyObject, value);
            }
        }
        private static bool IsNullableType(Type type) => type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));

        private static Expression StripQuotes(Expression node)
        {
            while (node.NodeType == ExpressionType.Quote)
            {
                node = ((UnaryExpression)node).Operand;
            }
            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            switch (node.Method.Name)
            {
                case "Where":
                    Visit(node.Arguments[1]);
                    break;
                case "Count":
                case "First":
                case "FirstOrDefault":
                    if (node.Arguments.Count == 1)
                    {
                        // FirstOrDefault() is called at the end of the query
                        Visit(node.Arguments[0]);
                    }
                    else
                    {
                        // FirstOrDefault() is called at the start of the query
                        Visit(node.Arguments[1]);
                    }
                    break;
                default:
                    throw new NotSupportedException(string.Format("The method '{0}' is not supported", node.Method.Name));
            }

            return node;
        }
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return base.VisitParameter(node);
        }
        protected override Expression VisitMember(MemberExpression node)
        {
            // we check if property do not have RedisKeyAttribute
            // every property that queries must have RedisKeyAttribute

            var property = node.Member.DeclaringType.GetProperty(node.Member.Name);
            if (property.GetCustomAttributes(typeof(RedisKeyAttribute), true).Count() != 1)
            {
                throw new NotSupportedException($"The property: {node.Member.Name} is not key, please use the property with RedisKeyAttribute or add RedisKeyAttribute to this property.");
            }
            _builderStack.Add(node.Member.Name);

            return node;
        }
        protected override Expression VisitBinary(BinaryExpression node)
        {
            Visit(node.Left);
            switch (node.NodeType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Equal:
                    break;
                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", node.NodeType));
            }
            Visit(node.Right);
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Value == null)
            {
                throw new NotSupportedException("The constant must have value");
            }
            _builderStack.Add(node.Value);
            return node;
        }
    }
}