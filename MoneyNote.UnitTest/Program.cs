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
        static ConcurrentDictionary<string, Task<string>> _cache = new ConcurrentDictionary<string, Task<string>>();
        static ConcurrentDictionary<string, bool> _cacheIsRunning = new ConcurrentDictionary<string, bool>();
        static ConcurrentDictionary<string, bool> _cacheIsDone = new ConcurrentDictionary<string, bool>();
        static List<string> result = new List<string>();
        static  void  Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //https://github.com/dotnet/runtime/issues/24293
            //https://codereview.stackexchange.com/questions/27507/is-this-code-thread-safe-singleton-implementation-using-concurrent-dictionary
            Stopwatch sw = Stopwatch.StartNew();

            Parallel.For(0, 10,  (i) =>
            {
                string value = $"begin_{i}_{DateTime.Now.Ticks}";
                Console.WriteLine(value);
                var x = _cache.GetOrAdd("test", BuildCache("test", i));
                var xr =  x.GetAwaiter().GetResult();
                result.Add($"{i}_Result_{xr}");
            });

            foreach(var r in result)
            {
                Console.WriteLine(r);
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.ReadLine();
        }

        public static async Task<string> BuildCache(string key,int i)
        {
            while(_cacheIsRunning.TryGetValue(key,out bool isRunning) && isRunning)
            {
                if(_cacheIsDone.TryGetValue(key,out bool isDone) && isDone)
                {
                    _cache.TryGetValue(key, out Task<string> valCached); return await valCached;
                }
                await Task.Delay(1);
            }

            _cacheIsRunning.GetOrAdd(key, true);

            if (_cache.TryGetValue(key, out Task<string> val)) return await val;

            string value = $"{i}_inner_{DateTime.Now.Ticks}";
            Console.WriteLine(value);
            //logic with take time here
            Thread.Sleep(2000);
            _cacheIsDone.GetOrAdd(key, true);
            return $"{i}_Data_{value    }";
        }
    }
}
