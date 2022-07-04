using System;
using System.Buffers;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
            // SizeOf<T>如果T为引用类型，则返回值是引用本身的大小(等于sizeof(void*)) 
            unsafe
            {
                int size = sizeof(void*);//8字节
            }
            int sizeOfMemoryInt = Unsafe.SizeOf<Memory<byte>>();//16字节
            int sizeOfMemorydecimal = Unsafe.SizeOf<Memory<decimal>>();//16字节
            int sizeOfIMemoryOwner = Unsafe.SizeOf<IMemoryOwner<int>>();//16字节

            int sizeOfMemoryDateTime = Unsafe.SizeOf<DateTime>();//8字节

            int sizeOfint = Unsafe.SizeOf<int>();//4
            int sizeOfstring = Unsafe.SizeOf<string>();//8字节
            int sizeOfobject = Unsafe.SizeOf<object>();//8字节
            int sizeOffloat = Unsafe.SizeOf<float>();//4字节
            int sizeOfdecimal = Unsafe.SizeOf<decimal>();//16字节
            int sizeOfdouble = Unsafe.SizeOf<double>();//8字节

            int sizeOfArrayBufferWriterdouble = Unsafe.SizeOf<ArrayBufferWriter<double>>();//8字节

            Arena arena = new Arena();
            int sizeOfArena = Unsafe.SizeOf<Arena>();//8字节
            Sequence<int> seq = arena.Allocate<int>(1024);

            Point xPoint = new Point(100, 100);
            Point yPoint;
            int sizeOfPoint = Marshal.SizeOf(xPoint);//8字节
            sizeOfPoint = Marshal.SizeOf(typeof(Point));//8字节
            IntPtr xPointPtr = Marshal.AllocHGlobal(Marshal.SizeOf(xPoint));
            Marshal.StructureToPtr(xPoint, xPointPtr, false);
            yPoint = (Point)Marshal.PtrToStructure(xPointPtr, typeof(Point));
            Marshal.FreeHGlobal(xPointPtr);

            int marshalSizeOf = Marshal.SizeOf(seq);
            Marshal.SizeOf(typeof(Sequence));
            int sizeOfSequence = Unsafe.SizeOf<Sequence>();//24
            OwnedArena<int> owned = arena.GetArena<int>();
            int sizeOfOwnedArena = Unsafe.SizeOf<OwnedArena<int>>();//8

            int sizeOfSequenceSegment = Unsafe.SizeOf<SequenceSegment<int>>();//8


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
