using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    public static class SpanDemo
    {
        private const string hello = "hello kitty";
        private static readonly byte[] hex = Encoding.ASCII.GetBytes("0123456789abcdef");
        public unsafe static long Sum()
        {
            fixed (byte* ptr = &hex[0])
            {
                var buffer = new Span<byte>(ptr, 100);
                var p = *(uint*)ptr;
            }
            fixed (byte* ptr = hex)
            {
                var p = *(ulong*)ptr;
            }
            Span<long> HighBits = new Span<long>() { };
            HighBits.Clear();
            HighBits[0] = unchecked((short)0x8080);
            HighBits[1] = unchecked((int)0x80808080);
            HighBits[2] = unchecked((long)0x8080808080808080L);
            

            Span<string> data = new Span<string>(new string[] { hello });
            ReadOnlySequence<byte> sequence = new ReadOnlySequence<byte>(hex);
            SequenceReader<byte> reader = new SequenceReader<byte>(sequence);
            reader.Advance(5);
            ReadOnlySequence<byte> trimmedBuffer = sequence.Slice(sequence.Start, sequence.End);
            var unreadSpan = reader.UnreadSpan;
            char[] array = hello.ToCharArray();
            Span<char> slice = array.AsSpan().Slice(6);
            hex.AsSpan().Slice(10);


            fixed (char* ptr = slice)
            {
                char* v = ptr + 2;
                ulong version = *(ulong*)ptr;
            }
            return int.Parse(slice);
        }
        public static async void Pipe()
        {
            PipeOptions inputOptions = new PipeOptions(
                new MemoryPoolDemo(),
                PipeScheduler.ThreadPool,
                PipeScheduler.Inline, 1024,
                2048, 64,
                false);
            Pipe pipe = new Pipe(inputOptions);
            ReadResult result = await pipe.Reader.ReadAsync();
        }
        public static int GetContentLength(ReadOnlySpan<char> span)
        {
            ReadOnlySpan<char> slice = span.Slice(16);
            return int.Parse(slice);
        }

        /// <summary>
        /// 使用stackalloc 关键字在堆栈上分配 100 字节的内存
        /// </summary>
        public static void WorkWithStackalloc()
        {
            // Create a span on the stack.
            Span<byte> stackSpan = stackalloc byte[100];
            InitSpan(stackSpan);
        }

        /// <summary>
        /// 表示任意内存的连续区域。 Span<T>实例通常用于保存数组或数组的一部分的元素。 但是，与数组不同， Span<T> 实例可以指向堆栈上托管的内存、本机内存或内存。 以下示例从数组创建一个 Span<Byte>
        /// </summary>
        public static void WorkWithArray()
        {
            // Create a span over an array.
            byte[] array = new byte[100];
            Span<byte> arraySpan = new Span<byte>(array);

            InitSpan(arraySpan);

        }

        /// <summary>
        /// 从100 字节的非托管内存创建一个 Span<Byte>
        /// </summary>
        public static void WorkWithAllocHGlobal()
        {
            // Create an array from native memory.
            IntPtr native = Marshal.AllocHGlobal(100);
            Span<byte> nativeSpan;
            unsafe
            {
                void* ptr = native.ToPointer();
                nativeSpan = new Span<byte>(ptr, 100);
            }
            InitSpan(nativeSpan);
            var sum = Sum(nativeSpan);
        }

        public static void InitSpan(Span<byte> span)
        {
            for (byte ctr = 0; ctr < span.Length; ctr++)
                span[ctr] = ctr;
        }
        public static long Sum(Span<byte> span)
        {
            long sum = 0;
            for (byte ctr = 0; ctr < span.Length; ctr++)
                sum += span[ctr];
            return sum;
        }
    }
}
