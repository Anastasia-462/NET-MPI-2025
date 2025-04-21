/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MT.Task5.Threads.SharedCollection
{
    class Program
    {
        static int CountElements = 10;
        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            var list = new List<int>();
            var autoResetEvent = new AutoResetEvent(false);
            var task_1 = Task.Run(() =>
            {
                var random = new Random();

                for (var i = 0; i < CountElements; i++)
                {
                    var randomValue = (int)random.Next(-100, 100);
                    list.Add(randomValue);
                    autoResetEvent.Set();
                    Thread.Sleep(300);
                }

                autoResetEvent.Set();
            });
            var task_2 = Task.Run(() =>
            {
                for (var i = 0; i < CountElements; i++)
                {
                    autoResetEvent.WaitOne();
                    Print(list);
                }
            });

            Console.ReadLine();
        }

        static void Print(List<int> list)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("[");
            for (var i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    stringBuilder.Append($"{list[i]}");
                }
                else
                {
                    stringBuilder.Append($"{list[i]}, ");
                }
            }
            stringBuilder.Append("]");

            Console.WriteLine(stringBuilder);
        }
    }
}
