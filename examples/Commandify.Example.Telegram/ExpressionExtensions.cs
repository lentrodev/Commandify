using System.Linq.Expressions;

namespace Commandify.Example.Telegram;

public static class ExpressionExtensions
{
    public static string GetName<T, T1>(Expression<Func<T, T1>> exp)
    {
        MemberExpression body = exp.Body as MemberExpression;

        if (body == null) {
            UnaryExpression unaryExpression = (UnaryExpression)exp.Body;
            body = unaryExpression.Operand as MemberExpression;
        }

        return body!.Member.Name;
    }
}