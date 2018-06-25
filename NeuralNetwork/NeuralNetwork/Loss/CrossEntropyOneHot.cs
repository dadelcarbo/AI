using System;

namespace NeuralNetwork.Loss
{
    public class CrossEntropyOneHot : ILossFunction
    {
        public string Name => "CrossEntropyOneHot";
        public double Evaluate(double[] actual, double[] expected, double[] errors)
        {
            if (actual.Length != expected.Length) throw new ArgumentException("Input arrays have different size");
            double crossEntropy = 0;
            for (int i = 0; i < expected.Length; i++)
            {
                if (expected[i] != 0)
                {
                    errors[i] = crossEntropy = -Math.Log(actual[i]);
                }
                else
                {
                    errors[i] = 0;
                }
            }
            return crossEntropy;
        }
    }
}