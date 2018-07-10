using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetwork.Activation;
using NeuralNetwork.Layer;
using NeuralNetwork.Loss;
using NeuralNetwork.MathTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkTest
{
    [TestClass]
    public class MatrixTest
    {
        private void Init(NNMatrix m)
        {
            for (int i = 1; i <= m.NbRows; i++)
            {
                for (int j = 1; j <= m.NbCols; j++)
                {
                    m[i - 1, j - 1] = i * 10 + j;
                }
            }
        }
        Random rnd = new Random();

        [TestMethod]
        public void MatrixConstructors()
        {
            var m1 = new NNMatrix(2, 3);
            Assert.AreEqual(m1.NbRows, 2);
            Assert.AreEqual(m1.NbCols, 3);
            Init(m1);

            var m2 = new NNMatrix(data);
            Assert.AreEqual(m2.NbRows, 2);
            Assert.AreEqual(m2.NbCols, 3);
            Assert.AreEqual(m1.ToString(), m2.ToString());
        }

        double[][] data = new double[2][]
        {
                new double [] {11,12,13},
                new double [] {21,22,23}
        };

        [TestMethod]
        public void MatrixRowCol()
        {
            var m1 = new NNMatrix(data);
            string s1 = m1.ToString();

            var m2 = new NNMatrix(2, 3);
            Init(m2);
            Assert.AreEqual(m1.ToString(), m2.ToString());

            NNMatrix M = new NNMatrix(2, 3); // 2 row * 3 Columns
            M[0, 0] = 11;
            M[0, 1] = 12;
            M[0, 2] = 13;
            M[1, 0] = 11;
            M[1, 1] = 12;
            M[1, 2] = 13;

            var s = M.ToString();

            Assert.AreEqual(2, m1.NbRows);
            Assert.AreEqual(2, M.NbRows);
            Assert.AreEqual(3, m1.NbCols);
            Assert.AreEqual(3, M.NbCols);
        }

        [TestMethod]
        public void MatrixMult()
        {
            NNMatrix M1 = NNMatrix.Random(2, 2); // 3 row * 4 Columns
            NNMatrix M2 = NNMatrix.Identity(2, 2);

            var M3 = M1 * M2;

            Assert.IsTrue(M1.Equals(M3));

            M1 = NNMatrix.Random(3, 4); // 3 row * 4 Columns
            M2 = NNMatrix.Random(4, 2);

            M3 = M1 * M2;

            Assert.AreEqual(M3.NbRows, 3);
            Assert.AreEqual(M3.NbCols, 2);
        }

        [TestMethod]
        public void MatrixCalc()
        {
            //  W*I+B  test calculattin Input*Weights + Biases
            int nbInput = 6;
            int nbOutput = 3;
            NNArray I = NNArray.Random(nbInput);
            NNMatrix W = NNMatrix.Random(nbInput, nbOutput);
            NNArray B = NNArray.Random(nbOutput);

            var tmp = W * I;
            var O = tmp + B;

            Assert.AreEqual(O.Length, nbOutput);
        }
        [TestMethod]
        public void MatrixPerformance()
        {
            //  W*I+B  test calculattin Input*Weights + Biases
            int nbInput = 300;
            int nbOutput = 200;

            NNArray I = NNArray.Random(nbInput);
            NNMatrix W = NNMatrix.Random(nbInput, nbOutput);
            NNArray B = NNArray.Random(nbOutput);

            int count = 200;
            DateTime start;
            DateTime end;
            double duration;

            // Not optimized
            start = DateTime.Now;
            for (int i = 0; i < count; i++)
            {
                NNArray res = W * I + B;
            }
            end = DateTime.Now;
            duration = (end - start).TotalMilliseconds / 1000;
            Console.WriteLine("Duration Non Optimized: "+duration);

            // Optimized
            NNArray O = NNArray.Random(nbOutput);
            start = DateTime.Now;
            for (int i = 0; i < count; i++)
            {
                W.Multiply(I,O);
                O.Add(B,O);
            }
            end = DateTime.Now;
            duration = (end - start).TotalMilliseconds / 1000;
            Console.WriteLine("Duration Optimized: " + duration);
        }
    }
}
