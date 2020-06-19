using System;
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
        static ConcurrentDictionary<string, bool> _cacheIsRunning = new ConcurrentDictionary<string, bool>();
        static ConcurrentDictionary<string, bool> _cacheIsDone = new ConcurrentDictionary<string, bool>();
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
               var xr = BuildCache("test", i);
               result.Add($"{i}_Result_{xr}");
           });

            foreach (var r in result)
            {
                Console.WriteLine(r);
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.ReadLine();
        }

        public static string BuildCache(string key, int i)
        {
            while (true)
            {
                if (_cacheIsDone.TryGetValue(key, out bool isDone) && isDone)
                {
                    if (_cache.TryGetValue(key, out string cached) && string.IsNullOrEmpty(cached) == false) return cached;
                }

                if (_cacheIsRunning.TryGetValue(key, out bool isRunning) && isRunning)
                {
                    if (_cacheIsDone.TryGetValue(key, out bool isDoneRecheck) && isDoneRecheck)
                    {
                        if (_cache.TryGetValue(key, out string cached) && string.IsNullOrEmpty(cached) == false) return cached;
                    }
                }
                else
                {
                    _cacheIsRunning.TryAdd(key, true);

                    string value = $"{i}_inner_{DateTime.Now.Ticks}";
                    //Console.WriteLine(value);
                    //logic with take time here
                    Thread.Sleep(2000);

                    _cacheIsDone.TryAdd(key, true);

                    _cache.TryAdd(key, value);

                    return $"{i}_Data_{value}";
                }
            }
        }
    }
}
