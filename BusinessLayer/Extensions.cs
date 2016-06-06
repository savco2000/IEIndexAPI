using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace BusinessLayer
{
    public static class Extensions
    {
        public static string GetEnumDescription(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[]) fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        //public static Collection<T> ToCollection<T>(this IEnumerable<T> items)
        //{
        //    var collection = new Collection<T>();

        //    items.ForEach(item => collection.Add(item));

        //    return collection;
        //}

        //public static Collection<T> ToCollection<T>(this IEnumerable<T> items)
        //{

        //    items.toCollection<T> collection = new Collection<T>();

        //    for (var i = 0; i < items.Count(); i++)
        //    {
        //        collection.Add(items[i]);
        //    }

        //    return collection;
        //}
    }
}
