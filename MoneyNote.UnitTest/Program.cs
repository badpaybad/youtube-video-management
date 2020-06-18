using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyNote.UnitTest
{
    class Program
    {
        static ConcurrentDictionary<string, string> _cache = new ConcurrentDictionary<string, string>();
        static List<string> result = new List<string>();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Parallel.For(0, 10, (i) =>
            {
                string value = $"begin_{i}_{DateTime.Now.Ticks}";
                Console.WriteLine(value);
                var x = _cache.GetOrAdd("test", BuildCache(i));
                result.Add($"{i}_Result_{x}");
            });

            foreach(var r in result)
            {
                Console.WriteLine(r);
            }
            Console.ReadLine();
        }

        public static string BuildCache(int i)
        {
            string value = $"{i}_inner_{DateTime.Now.Ticks}";
            Console.WriteLine(value);

            Thread.Sleep(2000);
            return $"{i}_Data_{value    }";
        }
    }
}
