
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace UniSpyServer.LinqToRedis.Linq
{
    public static class QueryEvaluator
    {
        /// <summary>
        /// Performs evaluation and replacement of independent sub-trees
        /// </summary>
        /// <param name="noderession">The root of the noderession tree.</param>
        /// <param name="fnCanBeEvaluated">A function that decides whether a given noderession node can be part of the local function.</param>
        /// <returns>A new tree with sub-trees evaluated and replaced.</returns>
        public static Expression PartialEval(Expression noderession, Func<Expression, bool> fnCanBeEvaluated)
        => new SubtreeEvaluator(new Nominator(fnCanBeEvaluated).Nominate(noderession)).Eval(noderession);

        /// <summary>
        /// Performs evaluation and replacement of independent sub-trees
        /// </summary>
        /// <param name="noderession">The root of the noderession tree.</param>
        /// <returns>A new tree with sub-trees evaluated and replaced.</returns>
        public static Expression PartialEval(Expression noderession) => PartialEval(noderession, QueryEvaluator.CanBeEvaluatedLocally);

        private static bool CanBeEvaluatedLocally(Expression noderession) => noderession.NodeType != ExpressionType.Parameter;

        /// <summary>
        /// Evaluates and replaces sub-trees when first candidate is reached (top-down)
        /// </summary>
        class SubtreeEvaluator : ExpressionVisitor
        {
            HashSet<Expression> candidates;
            internal SubtreeEvaluator(HashSet<Expression> candidates)
            {
                this.candidates = candidates;
            }
            internal Expression Eval(Expression node) => this.Visit(node);

            public override Expression Visit(Expression node)
            {
                if (node is null)
                {
                    return null;
                }
                if (this.candidates.Contains(node))
                {
                    return this.Evaluate(node);
                }
                return base.Visit(node);
            }
            private Expression Evaluate(Expression node)
            {
                if (node.NodeType == ExpressionType.Constant)
                {
                    return node;
                }
                LambdaExpression lambda = Expression.Lambda(node);
                Delegate fn = lambda.Compile();
                return Expression.Constant(fn.DynamicInvoke(null), node.Type);
            }
        }
        /// <summary>
        /// Performs bottom-up analysis to determine which nodes can possibly
        /// be part of an evaluated sub-tree.
        /// </summary>
        internal class Nominator : ExpressionVisitor
        {
            Func<Expression, bool> fnCanBeEvaluated;
            HashSet<Expression> candidates;
            bool cannotBeEvaluated;
            internal Nominator(Func<Expression, bool> fnCanBeEvaluated)
            {
                this.fnCanBeEvaluated = fnCanBeEvaluated;
            }
            internal HashSet<Expression> Nominate(Expression noderession)
            {
                this.candidates = new HashSet<Expression>();
                this.Visit(noderession);
                return this.candidates;
            }
            public override Expression Visit(Expression node)
            {
                if (node is not null)
                {
                    bool saveCannotBeEvaluated = this.cannotBeEvaluated;
                    this.cannotBeEvaluated = false;
                    base.Visit(node);
                    if (!this.cannotBeEvaluated)
                    {
                        if (this.fnCanBeEvaluated(node))
                        {
                            this.candidates.Add(node);
                        }
                        else
                        {
                            this.cannotBeEvaluated = true;
                        }
                    }
                    this.cannotBeEvaluated |= saveCannotBeEvaluated;
                }
                return node;
            }
        }
    }
}