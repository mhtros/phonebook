using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace phonebook_server.Extensions
{
    public static class Extensions
    {
        public static bool HasProperty(this Type type, string propertyName)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Any(p => string.Equals(p.Name, propertyName, StringComparison.CurrentCultureIgnoreCase));
        }

        public static void AddToList<TSource>(ref List<TSource> list, TSource value)
        {
            if (list == null) list = new List<TSource> { value };
            else list.Add(value);
        }
    }
}