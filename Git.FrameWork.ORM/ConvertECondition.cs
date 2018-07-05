namespace Git.Framework.ORM
{
    using System;

    public static class ConvertECondition
    {
        public static string ToType(ECondition condition)
        {
            string str = string.Empty;
            switch (condition)
            {
                case ECondition.Gth:
                    return ">";

                case ECondition.Lth:
                    return "<";

                case ECondition.Eth:
                    return "=";

                case ECondition.Geth:
                    return ">=";

                case ECondition.Leth:
                    return "<=";

                case ECondition.NotEth:
                    return "!=";

                case ECondition.Is:
                    return " IS ";

                case ECondition.IsNot:
                    return " IS NOT ";

                case ECondition.Between:
                case ECondition.OrLike:
                case ECondition.OrIn:
                case ECondition.OrBetween:
                    return str;

                case ECondition.OrGth:
                    return ">";

                case ECondition.OrLth:
                    return "<";

                case ECondition.OrEth:
                    return "=";

                case ECondition.OrGeth:
                    return ">=";

                case ECondition.OrLeth:
                    return "<=";

                case ECondition.OrNotEth:
                    return "!=";

                case ECondition.OrIs:
                    return " IS ";

                case ECondition.OrIsNot:
                    return " IS NOT ";

                case ECondition.AddEth:
                    return " += ";

                case ECondition.SubtractEth:
                    return " -= ";

                case ECondition.MultiplyEth:
                    return " *= ";

                case ECondition.DivideEth:
                    return " /= ";

                case ECondition.Modulo:
                    return " % ";

                case ECondition.Add:
                    return " + ";

                case ECondition.Subtract:
                    return " - ";

                case ECondition.Multiply:
                    return " * ";

                case ECondition.Divide:
                    return " / ";
            }
            return str;
        }
    }
}

