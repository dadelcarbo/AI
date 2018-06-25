using System;

namespace NeuralNetwork.Loss
{
    public class CrossEntropy : ILossFunction
    {
        public string Name => "CrossEntropy";
        public double Evaluate(double[] actual, double[] expected, double[] errors)
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