﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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
            //https://github.com/dotnet/runtime/issues/24293
            //https://codereview.stackexchange.com/questions/27507/is-this-code-thread-safe-singleton-implementation-using-concurrent-dictionary
            Stopwatch sw = Stopwatch.StartNew();

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

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.ReadLine();
        }

        public static string BuildCache(int i)
        {
            if (_cache.TryGetValue("test", out string val)) return val;

            string value = $"{i}_inner_{DateTime.Now.Ticks}";
            Console.WriteLine(value);

            Thread.Sleep(2000);
            return $"{i}_Data_{value    }";
        }
    }
}