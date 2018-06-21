using System;

namespace NeuralNetwork
{
    public class CrossEntropy : ILossFunction
    {
        public double Evaluate(double[] actual, double[] expected)
        {
            if (actual.Length != expected.Length) throw new ArgumentException("Input arrays have different size");
            double crossEntropy = 0;
            for (int i = 0; i < expected.Length; i++)
            {
                if (expected[i] != 0) crossEntropy -= Math.Log(actual[i]);
            }
            return crossEntropy;
        }
    }
}