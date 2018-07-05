using System;
using System.Runtime.CompilerServices;

namespace Git.Framework.Json
{
    public static class JsonObjectExtend
    {
        public static T AddProperty<T>(this T jsonObject, string PropertyName, object Value) where T: JsonObject
        {
            jsonObject.AddProperty(PropertyName, Value);
            return jsonObject;
        }

        public static T AddProperty<T>(this T jsonObject, string PropertyName, params object[] Values) where T: JsonObject
        {
            jsonObject.AddProperty(PropertyName, Values);
            return jsonObject;
        }
    }
}

