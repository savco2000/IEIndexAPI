using System;
using System.ComponentModel;

namespace BusinessLayer
{
    public static class Extensions
    {
        public static string GetEnumDescription(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[]) fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            var enumDescription = attributes.Length > 0 ? attributes[0].Description : value.ToString();

            return string.Equals(enumDescription, "Invalid", StringComparison.InvariantCultureIgnoreCase) ? null : enumDescription;
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
