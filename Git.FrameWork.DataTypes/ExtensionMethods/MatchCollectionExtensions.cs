namespace Git.Framework.DataTypes.ExtensionMethods
{
    using Git.Framework.DataTypes;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;

    public static class MatchCollectionExtensions
    {
        public static IEnumerable<Match> Where(this MatchCollection Collection, Predicate<Match> Predicate)
        {
            if (Collection.IsNull())
            {
                return null;
            }
            Predicate.ThrowIfNull("Predicate");
            GList<Match> list = new GList<Match>();
            foreach (Match match in Collection)
            {
                if (Predicate(match))
                {
                    list.Add(match);
                }
            }
            return list;
        }
    }
}

