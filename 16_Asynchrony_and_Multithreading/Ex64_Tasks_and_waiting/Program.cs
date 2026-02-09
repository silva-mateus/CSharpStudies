using System;

namespace Coding.Exercise
{
    public class Exercise
    {
        public static void RunSimpleTask()
        {
            Console.WriteLine("Before task starting.");

            Task task = Task.Run( () => {
                for (int i = 0; i < 3; i++)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine($"Iteration number {i+1}");
                }
            });

            task.Wait();
           
            Console.WriteLine("The task has finished.");
        }
    }
}
