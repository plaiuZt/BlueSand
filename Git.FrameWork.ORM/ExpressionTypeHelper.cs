namespace Git.Framework.ORM
{
    using System;
    using System.Linq.Expressions;

    public class ExpressionTypeHelper
    {
        public static ECondition ConvertECondition(ExpressionType expression)
        {
            ECondition eth = ECondition.Eth;
            switch (expression)
            {
                case ExpressionType.Equal:
                    return ECondition.Eth;

                case ExpressionType.ExclusiveOr:
                case ExpressionType.Invoke:
                case ExpressionType.Lambda:
                case ExpressionType.LeftShift:
                    return eth;

                case ExpressionType.GreaterThan:
                    return ECondition.Gth;

                case ExpressionType.GreaterThanOrEqual:
                    return ECondition.Geth;

                case ExpressionType.LessThan:
                    return ECondition.Lth;

                case ExpressionType.LessThanOrEqual:
                    return ECondition.Leth;

                case ExpressionType.NotEqual:
                    return ECondition.NotEth;
            }
            return eth;
        }

        public static string ConvertType(ExpressionType expression)
        {
            switch (expression)
            {
                case ExpressionType.Add:
                    return " + ";

                case ExpressionType.AddChecked:
                    return " + ";

                case ExpressionType.And:
                    return " AND ";

                case ExpressionType.AndAlso:
                    return " AND ";

                case ExpressionType.Divide:
                    return " / ";

                case ExpressionType.Equal:
                    return " = ";

                case ExpressionType.GreaterThan:
                    return " > ";

                case ExpressionType.GreaterThanOrEqual:
                    return " >= ";

                case ExpressionType.LessThan:
                    return " < ";

                case ExpressionType.LessThanOrEqual:
                    return " <= ";

                case ExpressionType.Multiply:
                    return " * ";

                case ExpressionType.MultiplyChecked:
                    return " * ";

                case ExpressionType.NotEqual:
                    return " <> ";

                case ExpressionType.Or:
                    return " OR ";

                case ExpressionType.OrElse:
                    return " OR ";

                case ExpressionType.Subtract:
                    return " - ";

                case ExpressionType.SubtractChecked:
                    return " - ";
            }
            return string.Empty;
        }
    }
}

