using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Pipelines.Sockets.Unofficial;
using Pipelines.Sockets.Unofficial.Arenas;
using Pipelines.Sockets.Unofficial.Buffers;
using StackExchange.Redis;

namespace TestConsole
{
    internal static class Program
    {
        public static async Task Main()
        {
            int sizeOfint = Unsafe.SizeOf<int>();
            int sizeOfstring = Unsafe.SizeOf<string>();
            int sizeOfobject = Unsafe.SizeOf<object>();
            int sizeOffloat = Unsafe.SizeOf<float>();

            Arena arena = new Arena();
            Sequence<int> seq = arena.Allocate<int>(1024);
            OwnedArena<int> owned = arena.GetArena<int>();

            ArrayPool<byte> pool = ArrayPool<byte>.Create(4096, 16);
            ArrayPoolAllocator<byte> arrayPoolAllocator = new ArrayPoolAllocator<byte>(pool);
            IMemoryOwner<byte> memoryOwner = arrayPoolAllocator.Allocate(512);
            Memory<byte> buffer = memoryOwner.Memory;
            buffer = new Memory<byte>(pool.Rent(1024));
            UnmanagedMemoryManager<byte> unmanagedMemory = new UnmanagedMemoryManager<byte>(buffer.Span);
            MemoryHandle pin = unmanagedMemory.Pin(64);
            Sequence<byte> s = new Sequence<byte>(buffer);
            s.AsReadOnly();
            Owned<long> owned1 = new Owned<long>(long.MaxValue, (x) => Console.WriteLine(x));
            owned1.Dispose();

            var client = ConnectionMultiplexer.Connect("localhost", new StreamWriter(Path.Combine(Environment.CurrentDirectory, "StackExchange.Redis.log")));
            client.GetDatabase().Ping();
            var db = client.GetDatabase(0);

            var start = DateTime.Now;

            Show(client.GetCounters());

            var tasks = Enumerable.Range(0, 1000).Select(async i =>
            {
                int timeoutCount = 0;
                RedisKey key = i.ToString();
                for (int t = 0; t < 1000; t++)
                {
                    try
                    {
                        await db.StringIncrementAsync(key, 1);
                    }
                    catch (TimeoutException) { timeoutCount++; }
                }
                return timeoutCount;
            }).ToArray();

            await Task.WhenAll(tasks);
            int totalTimeouts = tasks.Sum(x => x.Result);
            Console.WriteLine("Total timeouts: " + totalTimeouts);
            Console.WriteLine();
            Show(client.GetCounters());

            var duration = DateTime.Now.Subtract(start).TotalMilliseconds;
            Console.WriteLine($"{duration}ms");
        }
        private static void Show(ServerCounters counters)
        {
            Console.WriteLine("CA: " + counters.Interactive.CompletedAsynchronously);
            Console.WriteLine("FA: " + counters.Interactive.FailedAsynchronously);
            Console.WriteLine("CS: " + counters.Interactive.CompletedSynchronously);
            Console.WriteLine();
        }
    }
}
