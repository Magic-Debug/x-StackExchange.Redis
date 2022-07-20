using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    public ref struct UserInfo
    {
        public Span<int> Numbers { get; set; }

        public unsafe void Init()
        {
            int* numbers = stackalloc[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        }
    }
}
