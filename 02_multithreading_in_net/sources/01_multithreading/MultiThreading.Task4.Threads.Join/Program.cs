/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            CreateThreads(1, new ThreadResult { Value = 20 });

            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");
            CreateThreadPool(1, new ThreadResult { Value = 20 });
            Console.ReadLine();
        }

        static int ThreadMethod(ThreadResult threadResult)
        {
            threadResult.Value--;
            Console.WriteLine($"Value = {threadResult.Value}");
            return threadResult.Value;
        }

        static void CreateThreads(int threadNumber, ThreadResult threadResult)
        {
            if (threadNumber > 10)
            {
                return;
            }

            Console.Write($"Number of thread: {threadNumber}, ");

            var thread = new Thread(() => ThreadMethod(threadResult));
            thread.Start();
            thread.Join();
            threadNumber++;
            CreateThreads(threadNumber, threadResult);
        }

        static Semaphore sem = new Semaphore(1, 1);
        static void CreateThreadPool(int threadNumber, ThreadResult threadResult)
        {
            if (threadNumber > 10)
            {
                return;
            }

            ThreadPool.QueueUserWorkItem(_ =>
            {
                sem.WaitOne();
                Console.Write($"Number of thread: {threadNumber}, ");
                ThreadMethod(threadResult);
                sem.Release();
            });

            CreateThreadPool(threadNumber + 1, threadResult);
        }

        public class ThreadResult
        {
            public int Value { get; set; }
        }
    }
}
