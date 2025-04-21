/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            var tasks_1 = Task.Run(() =>
            {
                return CreateArray_Task1();
            });
            var tasks_2 = Task.Run(() =>
            {
                return MultiplyArray_Task2(tasks_1.Result);
            });

            var tasks_3 = Task.Run(() =>
            {
                SortArray_Task3(tasks_2.Result);
            });

            var tasks_4 = Task.Run(() =>
            {
                CalculateAverageValue_Task4(tasks_2.Result);
            });

            Console.ReadLine();
        }

        static int[] CreateArray_Task1()
        {
            var random = new Random();
            var array = Enumerable.Range(0, 10).Select(x => (int)random.Next(-100, 100)).ToArray();

            var stringBuilder = new StringBuilder();
            foreach (var value in array)
            {
                stringBuilder.Append($"{value} ");
            }

            Console.WriteLine($"Array values: {stringBuilder}");
            return array;
        }

        static int[] MultiplyArray_Task2(int[] array)
        {
            var random = new Random();
            var randomValue = (int)random.Next(-100, 100);

            var changedArray = array.Select(x => x * randomValue).ToArray();
            var stringBuilder = new StringBuilder();
            foreach (var value in changedArray)
            {
                stringBuilder.Append($"{value} ");
            }

            Console.WriteLine($"Array values multiplied by {randomValue}: {stringBuilder}");
            return changedArray;
        }

        static int[] SortArray_Task3(int[] array)
        {
            var sortedArray = array.OrderBy(x => x).ToArray();
            var stringBuilder = new StringBuilder();
            foreach (var value in sortedArray)
            {
                stringBuilder.Append($"{value} ");
            }

            Console.WriteLine($"Sorted array values: {stringBuilder}");
            return sortedArray;
        }

        static double CalculateAverageValue_Task4(int[] array)
        {
            var averageValue = array.Average();
            Console.WriteLine($"Average value: {averageValue}");

            return averageValue;
        }
    }
}
