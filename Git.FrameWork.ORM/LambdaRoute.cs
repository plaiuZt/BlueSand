namespace Git.Framework.ORM
{
    using System;
    using System.Linq.Expressions;
    using System.Text;

    public class LambdaRoute
    {
        public static string RouteMap(Expression expression)
        {
            if (!(expression is BinaryExpression))
            {
                if (expression is MemberExpression)
                {
                    MemberExpression expression2 = (MemberExpression) expression;
                    return expression2.Member.Name;
                }
                if (expression is NewArrayExpression)
                {
                    NewArrayExpression expression3 = (NewArrayExpression) expression;
                    StringBuilder builder = new StringBuilder();
                    foreach (Expression expression4 in expression3.Expressions)
                    {
                        builder.Append(RouteMap(expression4));
                        builder.Append(",");
                    }
                    return builder.ToString(0, builder.Length - 1);
                }
                if (expression is MethodCallExpression)
                {
                    MethodCallExpression expression5 = (MethodCallExpression) expression;
                    if (expression5.Method.Name == "Like")
                    {
                        return string.Format("({0} like {1})", RouteMap(expression5.Arguments[0]), RouteMap(expression5.Arguments[1]));
                    }
                    if (expression5.Method.Name == "NotLike")
                    {
                        return string.Format("({0} Not like {1})", RouteMap(expression5.Arguments[0]), RouteMap(expression5.Arguments[1]));
                    }
                    if (expression5.Method.Name == "In")
                    {
                        return string.Format("{0} In ({1})", RouteMap(expression5.Arguments[0]), RouteMap(expression5.Arguments[1]));
                    }
                    if (expression5.Method.Name == "NotIn")
                    {
                        return string.Format("{0} Not In ({1})", RouteMap(expression5.Arguments[0]), RouteMap(expression5.Arguments[1]));
                    }
                }
                else if (expression is ConstantExpression)
                {
                    ConstantExpression expression6 = (ConstantExpression) expression;
                    if (expression6.Value == null)
                    {
                        return "null";
                    }
                    if (expression6.Value is ValueType)
                    {
                        return expression6.Value.ToString();
                    }
                    if (((expression6.Value is string) || (expression6.Value is DateTime)) || (expression6.Value is char))
                    {
                        return string.Format("'{0}'", expression6.Value.ToString());
                    }
                }
                else if (expression is UnaryExpression)
                {
                    UnaryExpression expression7 = (UnaryExpression) expression;
                    return RouteMap(expression7.Operand);
                }
            }
            return string.Empty;
        }
    }
}

