using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using MultiThreading.Task3.MatrixMultiplier.Multipliers;

namespace MultiThreading.Task3.MatrixMultiplier.Tests
{
    [TestClass]
    public class MultiplierTest
    {
        [TestMethod]
        public void MultiplyMatrix3On3Test()
        {
            TestMatrix3On3(new MatricesMultiplier());
            TestMatrix3On3(new MatricesMultiplierParallel());
        }

        /// <summary>
        /// Best array size is 30.
        /// </summary>
        [TestMethod]
        public void ParallelEfficiencyTest()
        {
            int[] matrixSizes = { 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 140, 150, 160, 170, 180, 190, 200 };

            TimeSpan bestTimeMatricesMultiplier = TimeSpan.MaxValue;
            TimeSpan bestTimeMatricesMultiplierParallel = TimeSpan.MaxValue;
            int bestSizeMatricesMultiplier = 0;
            int bestSizeMatricesMultiplierParallel = 0;

            foreach (var size in matrixSizes)
            {
                var matricesMultiplier = new MatricesMultiplier();
                var matricesMultiplierParallel = new MatricesMultiplierParallel();

                var data = SeedData(size);
                var firstMatrix = data.firstMatrix;
                var secondMatrix = data.secondMatrix;

                var stopwatch = Stopwatch.StartNew();
                matricesMultiplier.Multiply(firstMatrix, secondMatrix);
                stopwatch.Stop();
                var matricesMultiplierTime = stopwatch.Elapsed;

                if (matricesMultiplierTime < bestTimeMatricesMultiplier)
                {
                    bestTimeMatricesMultiplier = matricesMultiplierTime;
                    bestSizeMatricesMultiplier = size;
                }

                stopwatch.Restart();
                matricesMultiplierParallel.Multiply(firstMatrix, secondMatrix);
                stopwatch.Stop();
                var matricesMultiplierParallelTime = stopwatch.Elapsed;

                if (matricesMultiplierParallelTime < bestTimeMatricesMultiplierParallel)
                {
                    bestTimeMatricesMultiplierParallel = matricesMultiplierParallelTime;
                    bestSizeMatricesMultiplierParallel = size;
                }

                Console.WriteLine($"Matrix Size: {size}, MatricesMultiplier Time: {matricesMultiplierTime.TotalMilliseconds} ms, MatricesMultiplierParallel Time: {matricesMultiplierParallelTime.TotalMilliseconds} ms");
            }

            Console.WriteLine($"Best Array Size for MatricesMultiplier: {bestSizeMatricesMultiplier} with Time: {bestTimeMatricesMultiplier.TotalMilliseconds} ms");
            Console.WriteLine($"Best Array Size for MatricesMultiplierParallel: {bestSizeMatricesMultiplierParallel} with Time: {bestTimeMatricesMultiplierParallel.TotalMilliseconds} ms");

            Assert.IsTrue(bestTimeMatricesMultiplierParallel < bestTimeMatricesMultiplier, "It was not possible to find the effective matrix size which makes parallel multiplication more effective than the regular one.");
        }

        #region private methods

        void TestMatrix3On3(IMatricesMultiplier matrixMultiplier)
        {
            if (matrixMultiplier == null)
            {
                throw new ArgumentNullException(nameof(matrixMultiplier));
            }

            var m1 = new Matrix(3, 3);
            m1.SetElement(0, 0, 34);
            m1.SetElement(0, 1, 2);
            m1.SetElement(0, 2, 6);

            m1.SetElement(1, 0, 5);
            m1.SetElement(1, 1, 4);
            m1.SetElement(1, 2, 54);

            m1.SetElement(2, 0, 2);
            m1.SetElement(2, 1, 9);
            m1.SetElement(2, 2, 8);

            var m2 = new Matrix(3, 3);
            m2.SetElement(0, 0, 12);
            m2.SetElement(0, 1, 52);
            m2.SetElement(0, 2, 85);

            m2.SetElement(1, 0, 5);
            m2.SetElement(1, 1, 5);
            m2.SetElement(1, 2, 54);

            m2.SetElement(2, 0, 5);
            m2.SetElement(2, 1, 8);
            m2.SetElement(2, 2, 9);

            var multiplied = matrixMultiplier.Multiply(m1, m2);
            Assert.AreEqual(448, multiplied.GetElement(0, 0));
            Assert.AreEqual(1826, multiplied.GetElement(0, 1));
            Assert.AreEqual(3052, multiplied.GetElement(0, 2));

            Assert.AreEqual(350, multiplied.GetElement(1, 0));
            Assert.AreEqual(712, multiplied.GetElement(1, 1));
            Assert.AreEqual(1127, multiplied.GetElement(1, 2));

            Assert.AreEqual(109, multiplied.GetElement(2, 0));
            Assert.AreEqual(213, multiplied.GetElement(2, 1));
            Assert.AreEqual(728, multiplied.GetElement(2, 2));
        }

        (Matrix firstMatrix, Matrix secondMatrix) SeedData(int matrixSize)
        {
            var firstMatrix = new Matrix(matrixSize, matrixSize);
            var secondMatrix = new Matrix(matrixSize, matrixSize);

            var random = new Random();
            for (var i = 0; i < matrixSize; i++)
            {
                for (var j = 0; j < matrixSize; j++)
                {
                    var randomValue = (int)random.Next(-100, 100);
                    firstMatrix.SetElement(i, j, randomValue);

                    randomValue = (int)random.Next(-100, 100);
                    secondMatrix.SetElement(i, j, randomValue);
                }
            }

            return (firstMatrix, secondMatrix);
        }
        #endregion
    }
}
