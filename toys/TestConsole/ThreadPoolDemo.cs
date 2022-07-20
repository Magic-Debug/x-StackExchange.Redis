using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestConsole
{
    public class ThreadPoolDemo : IThreadPoolWorkItem, ICriticalNotifyCompletion
    {

        public ThreadPoolDemo()
        {

        }

        public void Execute() => throw new NotImplementedException();



        public void OnCompleted(Action continuation) => throw new NotImplementedException();
        public void UnsafeOnCompleted(Action continuation) => throw new NotImplementedException();
    }
}
