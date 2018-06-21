using System;

namespace NeuralNetwork.Loss
{
    public class CrossEntropyOneHot : ILossFunction
    {
        public double Evaluate(double[] actual, double[] expected)
        {
            if (actual.Length != expected.Length) throw new ArgumentException("Input arrays have different size");
            double crossEntropy = 0;
            for (int i = 0; i < expected.Length; i++)
            {
                if (expected[i] != 0) {
                    return - Math.Log(actual[i]);
                }
            }
            return crossEntropy;
        }
    }
}