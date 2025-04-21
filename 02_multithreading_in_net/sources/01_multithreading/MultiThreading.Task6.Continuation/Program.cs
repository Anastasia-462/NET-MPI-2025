/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            PartA();
            Task.Delay(500).Wait();
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            PartB();
            Task.Delay(500).Wait();
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            PartC();
            Task.Delay(500).Wait();
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            PartD();
            Task.Delay(500).Wait();
            Console.ReadLine();
        }

        static void PartA()
        {
            #region ***With successful completion***
            //var parentTask_1 = Task.Run(() =>
            //{
            //    Console.WriteLine($"***With successful completion***");
            //    Console.WriteLine("Parent task started.");
            //    Console.WriteLine("Parent task completed.");
            //    Task.Delay(500).Wait();
            //});
            //var continuationTask_1 = parentTask_1.ContinueWith((task) =>
            //{
            //    if (task.IsFaulted)
            //    {
            //        Console.WriteLine($"Continuation Task: Parent task failed with exception: {task.Exception?.GetBaseException().Message}");
            //    }
            //    else if (task.IsCanceled)
            //    {
            //        Console.WriteLine("Continuation Task: Parent task was canceled.");
            //    }
            //    else
            //    {
            //        Console.WriteLine("Continuation Task: Parent task completed successfully.");
            //    }
            //}, TaskContinuationOptions.None);
            //Task.WaitAll(parentTask_1, continuationTask_1);
            #endregion

            #region ***With cancellation***
            //var cts = new CancellationTokenSource();
            //var parentTask_2 = Task.Run(() =>
            //{
            //    Console.WriteLine($"***With cancellation***");
            //    Console.WriteLine("Parent task started.");
            //    cts.Token.ThrowIfCancellationRequested();
            //}, cts.Token);
            //cts.Cancel();
            //var continuationTask_2 = parentTask_2.ContinueWith((task) =>
            //{
            //    if (task.IsFaulted)
            //    {
            //        Console.WriteLine($"Continuation Task: Parent task failed with exception: {task.Exception?.GetBaseException().Message}");
            //    }
            //    else if (task.IsCanceled)
            //    {
            //        Console.WriteLine("Continuation Task: Parent task was canceled.");
            //    }
            //    else
            //    {
            //        Console.WriteLine("Continuation Task: Parent task completed successfully.");
            //    }
            //    Task.Delay(500).Wait();
            //}, TaskContinuationOptions.None);
            //try
            //{
            //    Task.WaitAll(parentTask_2, continuationTask_2);
            //}
            //catch (AggregateException ex)
            //{
            //    foreach (var innerException in ex.InnerExceptions)
            //    {
            //        Console.WriteLine($"Caught exception from parent task: {innerException.Message}");
            //    }
            //}
            #endregion

            #region***With exception***
            var parentTask_3 = Task.Run(() =>
            {
                Console.WriteLine($"***With exception***");
                Console.WriteLine("Parent task started.");
                throw new InvalidOperationException("Something went wrong in the parent task.");
            });
            var continuationTask_3 = parentTask_3.ContinueWith((task) =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine($"Continuation Task: Parent task failed with exception: {task.Exception?.GetBaseException().Message}");
                }
                else if (task.IsCanceled)
                {
                    Console.WriteLine("Continuation Task: Parent task was canceled.");
                }
                else
                {
                    Console.WriteLine("Continuation Task: Parent task completed successfully.");
                }
            }, TaskContinuationOptions.None);
            try
            {
                Task.WaitAll(parentTask_3, continuationTask_3);
            }
            catch (AggregateException ex)
            {
                foreach (var innerException in ex.InnerExceptions)
                {
                    Console.WriteLine($"Caught exception from parent task: {innerException.Message}");
                }
            }
            #endregion

            Console.WriteLine("Program A completed.");
        }

        static void PartB()
        {
            #region ***With exception***
            var parentTask_1 = Task.Run(() =>
            {
                Console.WriteLine($"***With exception***");
                Console.WriteLine("Parent task started.");
                throw new InvalidOperationException("Something went wrong in the parent task.");
            });
            var continuationTask_1 = parentTask_1.ContinueWith((task) =>
            {
                Console.WriteLine($"Continuation Task: Parent task failed with exception: {task.Exception?.GetBaseException().Message}");
            }, TaskContinuationOptions.OnlyOnFaulted);

            try
            {
                Task.WhenAll(parentTask_1, continuationTask_1);
            }
            catch (AggregateException ex)
            {
                foreach (var innerException in ex.InnerExceptions)
                {
                    Console.WriteLine($"Caught exception from parent task: {innerException.Message}");
                }
            }
            #endregion

            #region ***With successful completion***
            //var parentTask_2 = Task.Run(() =>
            //{
            //    Console.WriteLine($"***With successful completion***");
            //    Console.WriteLine("Parent task started.");
            //    Console.WriteLine("Parent task completed.");
            //    Task.Delay(500).Wait();
            //});
            //var continuationTask_2 = parentTask_2.ContinueWith((task) =>
            //{
            //    Console.WriteLine($"Continuation Task: Parent task failed with exception: {task.Exception?.GetBaseException().Message}");
            //}, TaskContinuationOptions.OnlyOnFaulted);

            //Task.WhenAll(parentTask_2, continuationTask_2);
            #endregion

            Console.WriteLine("Program B completed.");
        }

        static void PartC()
        {
            #region ***With exception***
            var parentTask_1 = Task.Run(() =>
            {
                Console.WriteLine($"***With exception***");
                Console.WriteLine($"Parent Task started. Thread ID: {Thread.CurrentThread.ManagedThreadId}");
                Task.Delay(500).Wait();
                throw new InvalidOperationException("Something went wrong in the parent task.");
            });
            var continuationTask_1 = parentTask_1.ContinueWith((task) =>
            {
                Console.WriteLine($"Continuation Task started. Thread ID: {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine($"Continuation Task: Parent task failed with exception: {task.Exception?.GetBaseException().Message}");
            }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
            try
            {
                Task.WaitAll(parentTask_1, continuationTask_1);
            }
            catch (AggregateException ex)
            {
                foreach (var innerException in ex.InnerExceptions)
                {
                    Console.WriteLine($"Caught exception from parent task: {innerException.Message}");
                }
            }
            #endregion

            #region ***With successful completion***
            //var parentTask_2 = Task.Run(() =>
            //{
            //    Console.WriteLine($"***With successful completion***");
            //    Console.WriteLine($"Parent Task started. Thread ID: {Thread.CurrentThread.ManagedThreadId}");
            //    Console.WriteLine("Parent task completed.");
            //});
            //var continuationTask_2 = parentTask_2.ContinueWith((task) =>
            //{
            //    Console.WriteLine($"Continuation Task started. Thread ID: {Thread.CurrentThread.ManagedThreadId}");
            //    Console.WriteLine($"Continuation Task: Parent task failed with exception: {task.Exception?.GetBaseException().Message}");
            //}, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

            //Task.WaitAll(parentTask_2, continuationTask_2);
            #endregion

            Console.WriteLine("Program C completed.");
        }


        static void PartD()
        {
            #region ***With cancellation***
            var cts = new CancellationTokenSource();

            var parentTask_1 = Task.Run(() =>
            {
                Console.WriteLine($"***With cancellation***");
                Console.WriteLine($"Parent Task started. Thread ID: {Thread.CurrentThread.ManagedThreadId}");
                cts.Token.ThrowIfCancellationRequested();
                Console.WriteLine("Parent task completed.");
                Task.Delay(500).Wait();
            }, cts.Token);
            cts.Cancel();

            var continuationTask_1 = parentTask_1.ContinueWith((antecedent) =>
            {
                Task.Factory.StartNew(() =>
                {
                    Console.WriteLine($"Continuation Task executed using LongRunning. Thread ID: {Thread.CurrentThread.ManagedThreadId}");
                }, TaskCreationOptions.LongRunning).Wait();
            }, TaskContinuationOptions.OnlyOnCanceled);

            try
            {
                Task.WaitAll(parentTask_1, continuationTask_1);
            }
            catch (AggregateException ex)
            {
                Console.WriteLine($"Parent task was cancelled.");
            }

            #endregion

            #region ***With successful completion***
            //var parentTask_2 = Task.Run(() =>
            //{
            //    Console.WriteLine($"***With successful completion***");
            //    Console.WriteLine($"Parent Task started. Thread ID: {Thread.CurrentThread.ManagedThreadId}");
            //    Console.WriteLine("Parent task completed.");
            //});

            //var continuationTask_2 = parentTask_2.ContinueWith((antecedent) =>
            //{
            //    Task.Factory.StartNew(() =>
            //    {
            //        Console.WriteLine($"Continuation Task executed using LongRunning. Thread ID: {Thread.CurrentThread.ManagedThreadId}");
            //    }, TaskCreationOptions.LongRunning).Wait();
            //}, TaskContinuationOptions.OnlyOnCanceled);

            //Task.WaitAll(parentTask_2);
            #endregion
            Console.WriteLine("Program D completed.");
        }

    }
}
