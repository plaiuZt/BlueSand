namespace Git.Framework.ORM
{
    using Git.Framework.DataTypes;
    using System;
    using System.Linq.Expressions;

    public class LambdaProvider
    {
        public static Params<string, ECondition, object[]> BinaryProvider(Expression left, Expression right, ExpressionType expressionType)
        {
            MemberExpression operand;
            string name = string.Empty;
            if (left is MemberExpression)
            {
                operand = (MemberExpression) left;
                name = operand.Member.Name;
            }
            else if (left.NodeType == ExpressionType.Convert)
            {
                operand = ((UnaryExpression) left).Operand as MemberExpression;
                name = operand.Member.Name;
            }
            else
            {
                return null;
            }
            ECondition condition = ExpressionTypeHelper.ConvertECondition(expressionType);
            object[] objArray = null;
            if (right is ConstantExpression)
            {
                ConstantExpression expression2 = (ConstantExpression) right;
                if (expression2.Value == null)
                {
                    objArray = null;
                }
                else if (expression2.Value is ValueType)
                {
                    objArray = new object[] { expression2.Value };
                }
                else if (((expression2.Value is string) || (expression2.Value is DateTime)) || (expression2.Value is char))
                {
                    objArray = new object[] { expression2.Value };
                }
            }
            else if (right is MemberExpression)
            {
                operand = right as MemberExpression;
                if (operand.Type == typeof(DateTime))
                {
                    object obj2 = Expression.Lambda(operand, new ParameterExpression[0]).Compile().DynamicInvoke(new object[0]);
                    objArray = new object[] { obj2 };
                }
                else
                {
                    object obj3 = Expression.Lambda(operand, new ParameterExpression[0]).Compile().DynamicInvoke(new object[0]);
                    objArray = new object[] { obj3 };
                }
            }
            return new Params<string, ECondition, object[]> { Item1 = name, Item2 = condition, Item3 = objArray };
        }
    }
}

