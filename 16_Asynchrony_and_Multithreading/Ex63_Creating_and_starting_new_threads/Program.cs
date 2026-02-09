using System;

namespace Coding.Exercise
{
    public class Exercise
    {
        public static void RunThreads()
        {
            Thread thread1 = new Thread(() => PrintThreadID());
            Thread thread2 = new Thread(() => PrintThreadID());
            Thread thread3 = new Thread(() => PrintThreadID());

            thread1.Start();
            thread2.Start();
            thread3.Start();
        }

        public static void PrintThreadID()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"Hello from thread with ID: {threadId}");
        }
    }
}
