using System.Linq.Expressions;

namespace BeireMKit.Domain.Extensions
{
    public static class ExpressionExtension
    {
        public static Expression<Func<T, bool>> AndAlso<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
        {
            var parameterReplaceVisitor = new ParameterReplaceVisitor(left.Parameters[0], right.Parameters[0]);
            var rewrittenRight = parameterReplaceVisitor.Visit(right.Body);
            var andAlsoExpression = Expression.AndAlso(left.Body, rewrittenRight);
            return Expression.Lambda<Func<T, bool>>(andAlsoExpression, left.Parameters);
        }
    }

    public class ParameterReplaceVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _target;
        private readonly ParameterExpression _replacement;

        public ParameterReplaceVisitor(ParameterExpression target, ParameterExpression replacement)
        {
            _target = target;
            _replacement = replacement;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _target ? _replacement : base.VisitParameter(node);
        }
    }
}
