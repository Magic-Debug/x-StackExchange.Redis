using System;
using System.Buffers;

namespace TestConsole
{
    public class MemoryPoolDemo : MemoryPool<byte>
    {

        public MemoryPoolDemo()
        {

        }

        public override int MaxBufferSize => throw new NotImplementedException();

        public override IMemoryOwner<byte> Rent(int minBufferSize = -1) => throw new NotImplementedException();
        protected override void Dispose(bool disposing) => throw new NotImplementedException();
    }
}
