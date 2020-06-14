using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace MoneyNote.Core
{
    public static class EntityExtensions
    {
        public readonly static DateTime unixBeginDateTime = new DateTime(1970, 1, 1, 0, 0, 0);

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
        {
            return items.GroupBy(property).Select(x => x.First());
        }

        public static T CopyObject<T>(this T src)
        {
            var temp = JsonConvert.SerializeObject(src);

            return JsonConvert.DeserializeObject<T>(temp);
        }

        public static long ToUnixTimestamp(this DateTime d)
        {
            var epoch = d - unixBeginDateTime;

            return (long)epoch.TotalSeconds;
        }

        public static TimeSpan ToUnixTimeSpan(this DateTime d)
        {
            var epoch = d - unixBeginDateTime;

            return new TimeSpan(epoch.Ticks);
        }

        public static DateTime FromUnixTime(this long unixDateTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixDateTime).DateTime.ToLocalTime();
        }
        public static double ToDoubleTimestamp(this DateTime d)
        {
            var epoch = d - unixBeginDateTime;

            return epoch.TotalSeconds;
        }

        public static DateTime FromDoubleTime(this double unixDateTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds((long)unixDateTime).DateTime.ToLocalTime();
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(this List<T> items, Func<T, TKey> property)
        {
            return items.GroupBy(property).Select(x => x.First());
        }


        public static IEnumerable<T> MapToEntities<T>(this DbDataReader reader)
        {
            EntityDynamicBuilder<T> builder = EntityDynamicBuilder<T>.CreateBuilder(reader);
            while (reader.Read())
            {
                yield return builder.Build(reader);
            }
        }

        public static List<T> DataReaderMapToList<T>(this IDataReader dr)
        {
            List<T> list = new List<T>();
            while (dr.Read())
            {
                T obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!object.Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }

    }

}
